using ShimApi.ShimConfigMgr.Sections;

namespace ShimApi.ShimConfigMgr
{
    public class ApiConfig : IShimConfigurations
    {
        public EnvironmentConfigurations Environment { get; set; }
        public RimasConfigurations RimasConfigurations { get; set; }
        public SaiConfigurations SaiConfigurations { get; set; }

        public ApiConfig(IConfiguration config)
        {
            Environment = new EnvironmentConfigurations(config);
            RimasConfigurations = new RimasConfigurations(config.GetSection("Rimas"));
            SaiConfigurations = new SaiConfigurations(config.GetSection("Sai"));
        }
    }
}