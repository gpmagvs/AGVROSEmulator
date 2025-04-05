namespace AGVROSEmulator.IOModule
{
    public class SubmarineAGVIOModule : WagoIOModule
    {
        public override int LsrInStartIndex => (int)IOMap.OUTPUTS.Front_Protection_Sensor_IN_1;
        public override int SaftyRelayResetIndex => (int)IOMap.OUTPUTS.Safety_Relays_Reset;

        protected override void InitializeInputState()
        {
            SetState(IOMap.INPUTS.EMO, true);
            SetState(IOMap.INPUTS.Bumper_Sensor, true);
            SetState(IOMap.INPUTS.Horizon_Motor_Switch, true);
            //SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Vertical_Motor_Switch, true);
            SetState(IOMap.INPUTS.Horizon_Motor_Busy_1, true);
            SetState(IOMap.INPUTS.Horizon_Motor_Busy_2, true);
            SetState(IOMap.INPUTS.FrontProtection_Area_Sensor_1, true);
            SetState(IOMap.INPUTS.FrontProtection_Area_Sensor_2, true);
            SetState(IOMap.INPUTS.FrontProtection_Area_Sensor_3, true);
            SetState(IOMap.INPUTS.FrontProtection_Area_Sensor_4, true);
            SetState(IOMap.INPUTS.BackProtection_Area_Sensor_1, true);
            SetState(IOMap.INPUTS.BackProtection_Area_Sensor_2, true);
            SetState(IOMap.INPUTS.BackProtection_Area_Sensor_3, true);
            SetState(IOMap.INPUTS.BackProtection_Area_Sensor_4, true);
            SetState(IOMap.INPUTS.RightProtection_Area_Sensor_3, true);
            SetState(IOMap.INPUTS.LeftProtection_Area_Sensor_3, true);
            SetState(IOMap.INPUTS.Cst_Sensor_1, true);
            SetState(IOMap.INPUTS.Cst_Sensor_2, true);
            SetState(IOMap.INPUTS.Cst_Sensor_3, true);
            SetState(IOMap.INPUTS.Cst_Sensor_4, true);
            SetState(IOMap.INPUTS.FrontProtection_Obstacle_Sensor, true);
            SetState(IOMap.INPUTS.EQ_GO, true);
        }
    }
}
