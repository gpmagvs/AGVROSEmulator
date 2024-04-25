namespace AGVROSEmulator.IOModule
{
    public class DemoRoomInspectionAGVIOModule : WagoIOModule
    {
        public override int LsrInStartIndex => (int)IOMap.DEMO_INSPECTION_AGV_IOMAP_OUTPUTS.FrontProtectionSensorIN1;
        public override int SaftyRelayResetIndex => (int)IOMap.DEMO_INSPECTION_AGV_IOMAP_OUTPUTS.SafetyRelaysReset;

        protected override void InitializeInputState()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BumperSensor, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorSwitch, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery1LockSensor, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery2LockSensor, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor4, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor4, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.EQGO, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery1Exist2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery1Exist3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery2Exist2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery2Exist3, true);

        }


        public override void EmoButtonPush()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.EMOButton, true);
        }

        public override void Bumper(bool pressed)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BumperSensor, !pressed);
        }
        public override void MotorSwitch(bool opened)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorSwitch, opened);
        }
        public override void EMOButtonRelease()
        {
            base.EMOButtonRelease();
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.EMOButton, false);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.EMOButton2, false);
        }
        public override void MotorsAlarmReset()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.SaftyPLCOutput, false);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm1, false);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm2, false);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm3, false);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm4, false);
        }
        public override void MotorsAlarm()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.SaftyPLCOutput, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.HorizonMotorAlarm4, true);
        }
        public override void AllLaserAreaNoTrigger()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor4, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.BackProtectionAreaSensor4, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.RightProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.RightProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.RightProtectionAreaSensor3, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.LeftProtectionAreaSensor1, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.LeftProtectionAreaSensor2, true);
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.LeftProtectionAreaSensor3, true);
        }
        public override void FrontLaserArea3Trigger()
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.FrontProtectionAreaSensor3, false);
        }
        public void SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS item, bool state)
        {
            try
            {
                var index = (int)item;
                Console.WriteLine($"Set INPUT-{item}_0x{index.ToString("X2")}({index}) as {(state ? "1" : "0")}");
                slave.DataStore.InputDiscretes[index + 1] = state;
            }
            catch (Exception ex)
            {
            }
        }

        internal void SetBat1LockState(bool state)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery1LockSensor, state);
        }

        internal void SetBat2LockSensorState(bool state)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery2LockSensor, state);

        }

        internal void SetBat1UnlockState(bool state)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery1UnlockSensor, state);
        }

        internal void SetBat2UnlockSensorState(bool state)
        {
            SetState(IOMap.DEMO_INSPECTION_AGV_IOMAP_INPUTS.Battery2UnlockSensor, state);

        }
    }
}
