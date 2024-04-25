namespace AGVROSEmulator
{
    public abstract class Configurable
    {
        protected readonly IConfiguration Configuration;
        protected Configurable()
        {
            // 设置配置文件的路径
            var basePath = Directory.GetCurrentDirectory();

            // 配置builder来加载appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public string RosbridgeServerUrl => Configuration["RosbridgeSettings:ServerUrl"];
        public List<int> ChargeStationTags => Configuration.GetSection("Navigating:ChargStationTags").Get<List<int>>();

        public int LastVisitTag => Configuration.GetSection("Navigating:LastVisitTag").Get<int>();

        public AGV_TYPE AgvType
        {
            get
            {
                int typeInt = Configuration.GetSection("AGVType").Get<int>();
                AGV_TYPE enumValue = (AGV_TYPE)typeInt;
                return enumValue;
            }
        }
        public int WagoPort
        {
            get
            {
                return Configuration.GetValue<int>("WagoEmuSettings:Port");
            }
        }
    }
}
