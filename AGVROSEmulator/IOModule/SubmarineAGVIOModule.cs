namespace AGVROSEmulator.IOModule
{
    public class SubmarineAGVIOModule : WagoIOModule
    {
        public override int LsrInStartIndex => (int)IOMap.SUBMARINE_IOMAP_OUTPUTS.Front_Protection_Sensor_IN_1;
        public override int SaftyRelayResetIndex => (int)IOMap.SUBMARINE_IOMAP_OUTPUTS.Safety_Relays_Reset;

        protected override void InitializeInputState()
        {
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EMO, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Bumper_Sensor, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Switch, true);
            //SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Vertical_Motor_Switch, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_1, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Horizon_Motor_Busy_2, true);
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
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Cst_Sensor_1, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Cst_Sensor_2, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.EQ_GO, true);
        }
    }
}
