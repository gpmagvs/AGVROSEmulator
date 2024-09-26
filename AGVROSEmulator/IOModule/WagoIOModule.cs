using System.Net.Sockets;
using System.Net;
using Modbus.Device;
using RosSharp.RosBridgeClient;
using AGVROSEmulator.GPMRosMessageNet.SickSafetyscanners;
using System.Xml.Linq;
using RosSharp.RosBridgeClient.Actionlib;

namespace AGVROSEmulator.IOModule
{
    public abstract class WagoIOModule : Configurable
    {
        int InputStartAddress = 0;
        int OutputStartAddress = 512;
        public abstract int LsrInStartIndex { get; }

        public abstract int SaftyRelayResetIndex { get; }

        private OutputPathsMsg sickOutpuPathsMsg = new OutputPathsMsg();

        private int _CurrentLaserMode = -1;
        public int CurrentLaserMode
        {
            get => _CurrentLaserMode;
            protected set
            {
                if (_CurrentLaserMode != value)
                {
                    _CurrentLaserMode = value;
                    sickOutpuPathsMsg.active_monitoring_case = value;
                    Console.WriteLine($"Laser Mode CHanged to {value}");
                }
                PublishLaserData();
            }
        }
        public bool IsRechargeCircuitOpened => slave.DataStore.CoilDiscretes[OutputStartAddress + 1 + (int)IOMap.SUBMARINE_IOMAP_OUTPUTS.Recharge_Circuit];
        protected ModbusTcpSlave? slave;
        RosSocket rosSocket;
        string sickOutputPathsMsgID = "";
        public bool HSEqNoReplySimulation { get; private set; } = false;

        public WagoIOModule()
        {
        }
        public async Task<bool> Connect(string IP = "127.0.0.1", int Port = 502, byte DeviceID = 0x01)
        {
            rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosbridgeServerUrl));
            sickOutputPathsMsgID = rosSocket.Advertise<OutputPathsMsg>("/sick_safetyscanners/output_paths");
            try
            {
                IPAddress iPAddress = IPAddress.Parse(IP);
                TcpListener tcpListener = new TcpListener(iPAddress, Port);
                tcpListener.Start();

                slave = ModbusTcpSlave.CreateTcp(DeviceID, tcpListener);
                InitializeInputState();
                slave.ModbusSlaveRequestReceived += Slave_ModbusSlaveRequestReceived;
                WatchLaserModeCoils();
                WatchInputChanged();
                Task.Run(() =>
                {
                    slave.Listen();
                });
                GC.Collect();
                Console.WriteLine($"[WagoIOModule] Start (Port:{Port})");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[WagoIOModule] " + ex.Message);
                slave = null;
                return false;
            }
        }
        bool _AGV_COMPT = false;
        bool AGV_COMPT
        {
            get => _AGV_COMPT;
            set
            {
                if (_AGV_COMPT != value)
                {
                    _AGV_COMPT = value;
                    if (_AGV_COMPT)
                    {
                        SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_READY, false);
                        SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, false);
                        SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_U_REQ, false);
                        _wait_agv_busy_to_move_out = false;
                    }
                }
            }
        }

        bool _AGV_VALID = false;
        bool AGV_VALID
        {
            get => _AGV_VALID;
            set
            {
                if (_AGV_VALID != value)
                {
                    _AGV_VALID = value;
                    _AGVHSFlag = _AGV_VALID;
                    if (!_AGV_VALID)
                    {
                        _wait_agv_busy_to_move_out = false;
                    }
                    else
                    {
                        if (HSEqNoReplySimulation)
                            return;
                        SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, true);
                        SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_U_REQ, true);
                    }
                }
            }
        }
        bool _AGV_TR_REQ = false;
        bool AGV_TR_REQ
        {
            get => _AGV_TR_REQ;
            set
            {
                if (_AGV_TR_REQ != value)
                {
                    _AGV_TR_REQ = value;
                    SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_READY, _AGV_TR_REQ);
                }
            }
        }

        bool _AGVHSFlag = false;
        bool _wait_agv_busy_to_move_out = false;
        async Task WatchInputChanged()
        {
            bool[] previousInputs = new bool[64];
            while (true)
            {
                var inputs = slave.DataStore.CoilDiscretes.Skip(OutputStartAddress + 1).Take(64).ToArray();
                if (!previousInputs.SequenceEqual(inputs))
                {
                    if (inputs[(int)IOMap.SUBMARINE_IOMAP_OUTPUTS.Safety_Relays_Reset])
                    {
                        EMOReset();
                        MotorsAlarmReset();
                    }
                }

                AGV_VALID = inputs[(int)IOMap.SUBMARINE_IOMAP_OUTPUTS.AGV_VALID];

                if (_AGVHSFlag)
                {
                    AGV_TR_REQ = inputs[(int)IOMap.SUBMARINE_IOMAP_OUTPUTS.AGV_TR_REQ];
                    AGV_COMPT = inputs[(int)IOMap.SUBMARINE_IOMAP_OUTPUTS.AGV_COMPT];


                    if (inputs[(int)IOMap.SUBMARINE_IOMAP_OUTPUTS.AGV_READY] && !_wait_agv_busy_to_move_out)
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(100);
                            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_BUSY, true);
                            await Task.Delay(2000);
                            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_BUSY, false);
                            _wait_agv_busy_to_move_out = true;
                        });
                    }

                }


                previousInputs = inputs;
                await Task.Delay(50);
            }
        }
        void WatchLaserModeCoils()
        {
            Thread pubThread = new Thread(() =>
            {

                var _previous_coils_state_of_lsr = new List<bool>();
                while (true)
                {
                    int _address = OutputStartAddress + LsrInStartIndex + 1;
                    var _current = slave.DataStore.CoilDiscretes.Skip(_address).Take(8).ToList();


                    CurrentLaserMode = ToIntFromBooleans(_current.Take(8).ToArray());

                    _previous_coils_state_of_lsr = _current;
                    Thread.Sleep(100);
                }
            });
            pubThread.IsBackground = false;
            pubThread.Start();
        }
        async Task PublishLaserData()
        {
            rosSocket.Publish(this.sickOutputPathsMsgID, sickOutpuPathsMsg);
        }
        int ToIntFromBooleans(bool[] boolArray)
        {
            var _b = new bool[4] { boolArray[7], boolArray[5], boolArray[3], boolArray[1] };
            string str = string.Join("", _b.Select(b => b ? "1" : "0"));
            return Convert.ToInt32(str, 2);
        }
        protected abstract void InitializeInputState();
        public virtual void SetState(IOMap.SUBMARINE_IOMAP_INPUTS item, bool state)
        {
            try
            {
                var index = (int)item;
                slave.DataStore.InputDiscretes[index + 1] = state;
            }
            catch (Exception ex)
            {
            }
        }

        private byte _previous_function_code_requested = 0;
        private void Slave_ModbusSlaveRequestReceived(object? sender, ModbusSlaveRequestEventArgs e)
        {
            var inputs = slave.DataStore.InputDiscretes;
            var _functionCode = e.Message.FunctionCode;
            if (_functionCode != _previous_function_code_requested)
            {
                if (_functionCode == 15)
                {

                    var startAddress = e.Message.SlaveAddress;
                    var coils = slave.DataStore.CoilDiscretes;
                }
                //Console.WriteLine($"Function Code {_functionCode} requested.")
            }
            _previous_function_code_requested = _functionCode;
        }

        public void Disconnect()
        {
            slave.Dispose();
            slave = null;
        }

        public bool IsConnected()
        {
            return slave != null;
        }



        public virtual async void MotorsAlarm()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_1, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Alarm_1, true);

            await Task.Delay(400);

            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_2, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Alarm_2, true);
        }

        public virtual void MotorsAlarmReset()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_1, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_2, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Alarm_1, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Alarm_2, false);
        }

        public virtual void EmoButtonPush()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EMO, false);
            MotorsAlarm();
        }
        public virtual void EMOReset()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EMO, true);
        }

        public virtual void MotorSwitch(bool opened)
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Switch, opened);
        }


        public virtual async void ResetButtonPush()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Panel_Reset_PB, true);
            await Task.Delay(200);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Panel_Reset_PB, false);

            EMOReset();
            MotorsAlarmReset();
        }

        public virtual void EMOButtonRelease()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EMO, true);
        }

        public virtual void Bumper(bool pressed)
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Bumper_Sensor, !pressed);
            if (pressed)
                EmoButtonPush();
        }

        internal async void EQDownSimulation()
        {
            _AGVHSFlag = false;
            await Task.Delay(300);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_READY, false);
        }

        public virtual void Handshake_Signals_Reset()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_L_REQ, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_U_REQ, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_BUSY, false);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_READY, false);
        }
        public virtual void AllLaserAreaNoTrigger()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.FrontProtection_Area_Sensor_1, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.FrontProtection_Area_Sensor_2, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.FrontProtection_Area_Sensor_3, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.FrontProtection_Area_Sensor_4, true);

            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.BackProtection_Area_Sensor_1, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.BackProtection_Area_Sensor_2, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.BackProtection_Area_Sensor_3, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.BackProtection_Area_Sensor_4, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.RightProtection_Area_Sensor_3, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.LeftProtection_Area_Sensor_3, true);
        }
        public virtual void FrontLaserArea3Trigger()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.FrontProtection_Area_Sensor_3, false);
        }
        internal void PIO_HS_EQ_NO_REPLY()
        {
            HSEqNoReplySimulation = true;
        }

        internal void PIO_HS_EQ_NORMAL_REPLY()
        {
            HSEqNoReplySimulation = false;
        }
    }
}
