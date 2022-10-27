using System.Data.SqlClient;
using Dapper;
using SdiUtility;

namespace ShimApi.DataProviders
{
    public class MsSql: IMsSql
    {
        private readonly string _connectionStr;
    
    
        public MsSql(string connectionStr)
        {
            _connectionStr = connectionStr;
        }
    
    
        public (bool connected, string response) Version()
        {
            try
            {
                using SqlConnection conn = new(_connectionStr);
                conn.Open();
                string result = conn.Query<string>("SELECT CONVERT(varchar, GETDATE(), 127)").FirstOrDefault() ?? string.Empty;
                return (result.IsNotEmpty(), result.IsNotEmpty() ? $"Connected as of {result}" : "Not Connected");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }
    }
}