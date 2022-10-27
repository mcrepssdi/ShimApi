namespace ShimApi.ShimConfigMgr.Sections
{
    public class SaiDatabase : IDatabase
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionStr { get; set; }
        public string Schema { get; set; }

        public SaiDatabase(IConfiguration section)
        {
        
            Server = section.GetValue<string>("Server");
            Port = section.GetValue<int>("Port");
            User = section.GetValue<string>("User");
            Password = section.GetValue<string>("Password");
            Schema = section.GetValue<string>("Schema");
        
            ConnectionStr = $"Server={Server};Database={Schema};Uid={User};Pwd={Password};";
        }
    }
}