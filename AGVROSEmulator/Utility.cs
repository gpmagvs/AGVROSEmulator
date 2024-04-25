using AGVROSEmulator.AGVRos;
using AGVROSEmulator.IOModule;

namespace AGVROSEmulator
{
    public class Utility
    {
        public static AGVEmu AGVEmulator = new AGVEmu();
        public static WagoIOModule IOModule;
    }

    public class ConfigurationHelper : Configurable
    {
        public static ConfigurationHelper Instance = new ConfigurationHelper();
    }

    public enum AGV_TYPE
    {
        FORK,
        SUBMARINE,
        INSPECTION,
        DEMO_INSPECTION
    }


}
