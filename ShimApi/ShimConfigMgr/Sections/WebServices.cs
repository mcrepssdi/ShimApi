namespace ShimApi.ShimConfigMgr.Sections
{
    public class WebServices
    {
        public string InternalServerName { get; set; }
        public string InternalServicePath { get; set; }

        public WebServices(IConfiguration section)
        {
            InternalServerName = section.GetValue<string>("InternalServerName");
            InternalServicePath = section.GetValue<string>("InternalServicePath");
        }
    }
}