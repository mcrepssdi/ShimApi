namespace ShimApi.ShimConfigMgr.Sections
{
    public class EnvironmentConfigurations
    {
        public string ApplicationName { get; set; }
        public string ApplicationId { get; set; }
        public string AppServer { get; set; }
        public string AppDb { get; set; }
        public string LogDirectory { get; set; }
        public string BaseDirectory { get; set; }
        public string ConnectionStr { get; set; }
        public int MaxAttemptsForControlNo { get; set; }

        public EnvironmentConfigurations(IConfiguration config)
        {
            ApplicationName = config.GetValue<string>("ApplicationName");
            ApplicationId = config.GetValue<string>("ApplicationId");
            AppDb = config.GetValue<string>("AppDb");
            MaxAttemptsForControlNo = config.GetValue<int>("MaxAttemptsForControlNo");
       
            AppServer = config.GetValue<string>("AppServer");
            BaseDirectory = config.GetValue<string>("BaseDirectory");
            LogDirectory = Path.Combine(BaseDirectory, config.GetValue<string>("LogDirectory"), config.GetValue<string>("ApplicationName"));
            ConnectionStr =
                $"Server={AppServer};Database={AppDb};Trusted_Connection=True;";
        }
    }
}