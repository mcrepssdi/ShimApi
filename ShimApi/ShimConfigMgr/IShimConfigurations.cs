using ShimApi.ShimConfigMgr.Sections;

namespace ShimApi.ShimConfigMgr
{
    public interface IShimConfigurations
    {
        public EnvironmentConfigurations Environment { get; set; }
        public RimasConfigurations RimasConfigurations { get; set; }
        public SaiConfigurations SaiConfigurations { get; set; }
    }
}