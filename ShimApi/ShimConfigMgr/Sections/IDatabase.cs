namespace ShimApi.ShimConfigMgr.Sections
{
    public interface IDatabase
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ConnectionStr { get; set; }
        public string Schema { get; set; }
    }
}