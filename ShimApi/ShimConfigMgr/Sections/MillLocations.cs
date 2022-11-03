using ShimApi.Enumerations;
using ShimApi.Utilities;

namespace ShimApi.ShimConfigMgr.Sections
{
    public class MillLocations
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public List<string> Schemas { get; set; }
        public string ConnectionStr { get; set; }
    
        public string IntegrationUser { get; set; }
        public string IntegrationPassword { get; set; }
        public string IntegrationConnectionStr { get; set; }
    
        public MillLocations(IConfiguration section)
        {
            User = section.GetValue<string>("User");
            Password = section.GetValue<string>("Password").GetPassword();
            Server = section.GetValue<string>("Server");
            Port = section.GetValue<int>("Port");
            Schemas = new List<string>(section.GetValue<string>("Schemas").Split(","));
            if (Schemas.Count == 0)
            {
                throw new Exception("No Schema's found.");
            }
            ConnectionStr = SetConnectionString(User, Password);
        
            IntegrationUser = section.GetValue<string>("IntegrationUser");
            IntegrationPassword = section.GetValue<string>("IntegrationPassword").GetPassword();
            IntegrationConnectionStr = SetConnectionString(IntegrationUser, IntegrationPassword);
        }

        private string SetConnectionString(string user, string pwd)
        {
            return $"Server={Server};Database={Schemas[0]};Uid={user};Pwd={pwd};";
        }

        public string GetConnectionString(SoftwareVendor softwareVendor)
        {
            return softwareVendor switch
            {
                SoftwareVendor.Rimas => IntegrationPassword,
                _ => ConnectionStr
            };
        }
    }
}