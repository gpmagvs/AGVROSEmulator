namespace AGVROSEmulator.IOModule
{
    public class ForkAGVIOModule : SubmarineAGVIOModule
    {
        protected override void InitializeInputState()
        {
            base.InitializeInputState();
            SetState(IOMap.INPUTS.Vertical_Motor_Switch, true);
            SetState(IOMap.INPUTS.Vertical_Motor_Busy, true);
            SetState(IOMap.INPUTS.Vertical_Home_Pose, true);

        }
    }
}
