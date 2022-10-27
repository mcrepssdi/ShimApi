namespace ShimApi.ShimConfigMgr.Sections
{
    public class SaiConfigurations
    {
        public IDatabase Database { get; set; }
        public WebServices WebServices { get; set; }

        public MillLocations MillLocations { get; set; }
    
        public SaiConfigurations(IConfiguration section)
        {
            Database = new SaiDatabase(section.GetSection("Omnisource"));
            WebServices = new WebServices(section.GetSection("WebServices"));
            MillLocations = new MillLocations(section.GetSection("SdiMills"));
        }
    }
}