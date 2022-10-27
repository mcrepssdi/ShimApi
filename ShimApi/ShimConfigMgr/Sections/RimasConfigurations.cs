namespace ShimApi.ShimConfigMgr.Sections
{
    public class RimasConfigurations
    {
        public RimasApi RimasAPI { get; set; }


        public RimasConfigurations(IConfiguration config)
        {
            RimasAPI = new RimasApi(config.GetSection("RimasApi"));
        }
    
    }
}