using System.Text;
using Dapper;
using SdiUtility;
using ShimApi.Enumerations;

namespace ShimApi.DataProviders
{
    public static class SaiQueryBuilder
    {

        /// <summary>
        /// Fetches a specified Service Property from the Sai Master Schema
        /// </summary>
        /// <param name="property">Desired Property name</param>
        /// <returns></returns>
        public static (string sql, DynamicParameters dp) ServicePropertyQuery(string property)
        {
            const string sql = "SELECT Value AS Property FROM saimaster.SAI_Webservice_Prop WHERE Property = @Property;";
            DynamicParameters dp = new();
            dp.Add("@Property", property);
            return (sql, dp);
        }
        
        /// <summary>
        /// Fetches the SAI API Token from the Sai Master Schema
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="serviceName"></param>
        /// <param name="serviceMethod"></param>
        /// <returns></returns>
        public static (string sql, DynamicParameters dp) ServiceTokenQuery(string schema, string serviceName, string serviceMethod)
        {
            StringBuilder sb = new();
            sb.Append("SELECT Token FROM saimaster.SAI_Webservice_Token ");
            sb.AppendLine("WHERE Env = @Environment AND Service_Name = @ServiceName AND Service_Method = @ServiceMethod;");

            DynamicParameters dp = new();
            dp.Add("@Environment", schema);
            dp.Add("@ServiceName", serviceName);
            dp.Add("@ServiceMethod", serviceMethod);
            return (sb.ToString(), dp);
        }

        public static (string sql, DynamicParameters dp) InternalServerPathQuery(string schema, string serviceName, string serviceMethod, string branch)
        {
            StringBuilder sb = new();
            sb.Append("SELECT Property, Value FROM saimaster.SAI_Webservice_Prop ");
            sb.AppendLine("WHERE Env = @Environment AND Property IN (@InternalServerName, @InternalServicePath) ");
            sb.AppendLine("AND Branch = @Branch AND Service_Name = @ServiceName AND Service_Method = @ServiceMethod");
        
            DynamicParameters dp = new();
            dp.Add("@Environment", schema);
            dp.Add("@ServiceName", serviceName);
            dp.Add("@ServiceMethod", serviceMethod);
            dp.Add("@Branch", branch);
            dp.Add("@InternalServerName", ApiServiceEnums.InternalServerName.GetDescription());
            dp.Add("@InternalServicePath", ApiServiceEnums.InternalServicePath.GetDescription());
        
            return (sb.ToString(), dp);
        }
    }
}