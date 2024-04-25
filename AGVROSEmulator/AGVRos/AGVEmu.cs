using AGVROSEmulator.GPMRosMessageNet.Actions;
using AGVROSEmulator.GPMRosMessageNet.Messages;
using AGVROSEmulator.GPMRosMessageNet.Services;
using AGVROSEmulator.IOModule;
using Newtonsoft.Json;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;

namespace AGVROSEmulator.AGVRos
{
    public class AGVEmu : Configurable
    {
        ModuleInformation moduleInfo = new ModuleInformation();
        RosSocket rosSocket;
        TaskCommandActionServer actionServer;

        WagoIOModule IOModule => Utility.IOModule;
        public void Initialize()
        {
            rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosbridgeServerUrl));
            rosSocket.protocol.OnClosed += Protocol_OnClosed;
            InitModuleInfoData();
            AdvertiseServices();
            InitNewTaskCommandActionServer();
            PublishModuleInfo();
        }

        private void Protocol_OnClosed(object? sender, EventArgs e)
        {
            Console.WriteLine("Ros斷線 嘗試重新初始化");
            Initialize();
        }

        private void InitModuleInfoData()
        {
            moduleInfo = new ModuleInformation()
            {
                nav_state = new NavigationState
                {
                    lastVisitedNode = new RosSharp.RosBridgeClient.MessageTypes.Std.Int32(50),
                    robotPose = new RosSharp.RosBridgeClient.MessageTypes.Geometry.PoseStamped()
                    {
                    },
                    robotDirect = 0

                },
                Battery = new BatteryState()
                {
                    batteryLevel = 99,
                    batteryID = 0
                },
                reader = new BarcodeReaderState()
                {
                    tagID = 50,
                }
            };
        }

        private async void PublishModuleInfo()
        {
            string id = rosSocket.Advertise<ModuleInformation>("/module_information");
            await Task.Delay(1).ConfigureAwait(false);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    moduleInfo.nav_state.lastVisitedNode.data = actionServer.lastVisitedTag;
                    moduleInfo.nav_state.robotPose.pose.position = actionServer.currentCoordination;
                    moduleInfo.nav_state.robotPose.pose.orientation = actionServer.orientation;
                    moduleInfo.reader.tagID = (uint)actionServer.barcodeReadTag;
                    moduleInfo.reader.theta = actionServer.currentAngle;
                    rosSocket.Publish(id, moduleInfo);
                    await Task.Delay(100).ConfigureAwait(false);
                }
            });
        }

        private void HandleModuleInformation(ModuleInformation t)
        {
            //Console.WriteLine(JsonConvert.SerializeObject(t, Formatting.Indented));
        }

        /// <summary>
        /// 建立服務
        /// </summary>
        internal virtual void AdvertiseServices()
        {
            rosSocket.AdvertiseService<CSTReaderCommandRequest, CSTReaderCommandResponse>("/CSTReader_action", CstReaderServiceCallack);
            rosSocket.AdvertiseService<VerticalCommandRequest, VerticalCommandResponse>("/command_action", VerticalActionCallback);
            rosSocket.AdvertiseService<VerticalCommandRequest, VerticalCommandResponse>("/command_actionm", BatteryLockActionRequestHandler);
            rosSocket.AdvertiseService<SetcurrentTagIDRequest, SetcurrentTagIDResponse>("/set_currentTagID", SetCurrentTagRequestHandler);
        }

        /// <summary>
        /// 建立 barcodemovebase server
        /// </summary>
        internal virtual void InitNewTaskCommandActionServer()
        {
            actionServer = new TaskCommandActionServer("/barcodemovebase", rosSocket);
            actionServer.Initialize();
            actionServer.SetChargeStationTags(this.ChargeStationTags);
            actionServer.lastVisitedTag = this.LastVisitTag;
            actionServer.OnAGVParkAtChargeStation += ActionServer_OnAGVParkAtChargeStation;
            actionServer.OnAGVLeaveChargeStation += ActionServer_OnAGVLeaveChargeStation;

            Console.WriteLine("ROS Enum Action Server Created!");
        }

        private void ActionServer_OnAGVLeaveChargeStation(object? sender, EventArgs e)
        {

            moduleInfo.Battery.chargeCurrent = 0;
            moduleInfo.Battery.dischargeCurrent = 2320;
        }

        private void ActionServer_OnAGVParkAtChargeStation(object? sender, EventArgs e)
        {
            if (IOModule.IsRechargeCircuitOpened)
            {
                moduleInfo.Battery.dischargeCurrent = 0;
                moduleInfo.Battery.chargeCurrent = 2000;
            }
        }

        private bool BatteryLockActionRequestHandler(VerticalCommandRequest tin, out VerticalCommandResponse tout)
        {
            tout = new VerticalCommandResponse()
            {
                confirm = true,
            };

            return true;
        }

        private bool VerticalActionCallback(VerticalCommandRequest tin, out VerticalCommandResponse tout)
        {
            tout = new VerticalCommandResponse()
            {
                confirm = true,
            };

            return true;
        }

        private bool CstReaderServiceCallack(CSTReaderCommandRequest tin, out CSTReaderCommandResponse tout)
        {
            tout = new CSTReaderCommandResponse()
            {
                confirm = true
            };

            return true;
        }


        private bool SetCurrentTagRequestHandler(SetcurrentTagIDRequest tin, out SetcurrentTagIDResponse tout)
        {
            Console.WriteLine("定位:" + JsonConvert.SerializeObject(tin));
            tout = new SetcurrentTagIDResponse(true);

            actionServer.lastVisitedTag = tin.tagID;
            actionServer.currentCoordination = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Point(tin.X, tin.Y, 0);
            actionServer.currentAngle = tin.angle;
            actionServer.barcodeReadTag = tin.tagID;

            Console.WriteLine($"{actionServer.lastVisitedTag} ({actionServer.currentCoordination.x},{actionServer.currentCoordination.y})");
            return true;
        }

    }
}
