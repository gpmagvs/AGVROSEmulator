using RosSharp.RosBridgeClient;


namespace AGVROSEmulator.GPMRosMessageNet.Services
{
    public class TaskDoneCommandResponse : Message
    {
        public const string RosMessageName = "gpm_msgs/TaskDoneCommand";

        public bool confirm { get; set; }

        public TaskDoneCommandResponse()
        {
            this.confirm = false;
        }

        public TaskDoneCommandResponse(bool confirm)
        {
            this.confirm = confirm;
        }
    }
}
