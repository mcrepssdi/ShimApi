using ShimApi.ShimConfigMgr.Sections;

namespace ShimApi.Models
{
    public class ApiProperties
    {
        public string DataEnvironmentName { get; init; } = string.Empty;
        public ApiParameters ApiParameters { get; init; } = new();
        public RimasApi RimasApi { get; init; } = new();
    }
}
