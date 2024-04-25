namespace AGVROSEmulator.IOModule
{
    public class ForkAGVIOModule : SubmarineAGVIOModule
    {
        protected override void InitializeInputState()
        {
            base.InitializeInputState();
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Vertical_Motor_Switch, true);
            SetState(IOMap.SUBMARINE_IOMAP_INPUTS.Vertical_Motor_Busy, true);


        }
    }
}
