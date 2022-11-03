using ShimApi.Models;
using ShimApi.Utilities;

namespace ShimApi.ShimConfigMgr.Sections
{
    public class RimasApi: ApiParameters
    {
        public string Schema { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string RimasUser { get; set; } = string.Empty;
        public string RimasPwd { get; set; } = string.Empty;
    
        public RimasApi() { }
        public RimasApi(IConfiguration section)
        {
            Schema = section.GetValue<string>("Schema");
            Company = section.GetValue<string>("Company");
            Division = section.GetValue<string>("Division");
            RimasUser = section.GetValue<string>("RimasUser");
            ServiceUrl = section.GetValue<string>("ServiceUrl");
            ServicePath = section.GetValue<string>("ServicePath");
            RimasPwd = section.GetValue<string>("RimasPwd").GetPassword();
        }


    }
}