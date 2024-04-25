/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using RosSharp.RosBridgeClient;


namespace AGVROSEmulator.GPMRosMessageNet.SickSafetyscanners
{
    public class IntrusionDataMsg : Message
    {
        public const string RosMessageName = "sick_safetyscanners/IntrusionDataMsg";

        public IntrusionDatumMsg[] data { get; set; }

        public IntrusionDataMsg()
        {
            this.data = new IntrusionDatumMsg[0];
        }

        public IntrusionDataMsg(IntrusionDatumMsg[] data)
        {
            this.data = data;
        }
    }
}
