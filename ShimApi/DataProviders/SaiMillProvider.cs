using System.Text;
using Dapper;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NLog;
using SdiUtility;
using ShimApi.Models;
using ShimApi.Models.SalesShipmentModels;

namespace ShimApi.DataProviders
{
    public class SaiMillProvider : IMySqlMillProvider
    {
        private readonly string _connectionStr;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    
        public SaiMillProvider(string connectionStr)
        {
            _connectionStr = connectionStr;
        }

        /// <inheritdoc cref="IMySqlMillProvider.WebServicePropertyHierarchical"/>
        public string WebServicePropertyHierarchical(ServiceAware serviceAware, string property )
        {
            ValidatePost validPost = (ValidatePost) serviceAware;
        
            _logger.Trace($"Fetching Value: ValidatePost: {JsonConvert.SerializeObject(validPost)}");
            _logger.Trace($"Fetching Value: ServiceName: {serviceAware.ServiceName}, ServiceMethod: {serviceAware.ServiceMethod} Parameter: {property}");

            StringBuilder sb = new ();
            sb.AppendLine("SELECT Value ");
            sb.AppendLine(" FROM saimaster.SAI_Webservice_Prop ");
            sb.AppendLine("   WHERE (Property = @Property OR Property = @SupplierNo) ");
            sb.AppendLine("   AND Env in ( '*', @Env ) ");
            sb.AppendLine("   AND Branch in ( '*', @Branch )");
            sb.AppendLine("   AND Service_Name  =  @ServiceName ");
            sb.AppendLine("   AND Service_Method  = @ServiceMathod ");
            sb.AppendLine(" ORDER BY Env DESC, Branch DESC, Service_Name DESC, Service_Method DESC LIMIT 1 ");
        
            DynamicParameters dp = new();
            dp.Add("@Property", property);
            dp.Add("@SupplierNo", validPost.SupplierNo);
            dp.Add("@Env", validPost.DataEnvironmentName);
            dp.Add("@Branch", validPost.Branch);
            dp.Add("@ServiceName", serviceAware.ServiceName);
            dp.Add("@ServiceMathod", serviceAware.ServiceMethod);
        
            try
            {
                using MySqlConnection conn = new (_connectionStr);
                conn.Open();
                string result = conn.Query<string>(sb.ToString(), dp).FirstOrDefault() ?? string.Empty;
                _logger.Trace("Finished Successfully...");
            
                return result;
            }
            catch (Exception e)
            {
                _logger.Error($"Error fetching Parameter for {property}");
                _logger.Error(e.Message);
            }
        
            return string.Empty;
        }

        /// <inheritdoc cref="IMySqlMillProvider.IsUnique"/>
        public bool IsUnique(string value, string dataEnvironmentName)
        {
            _logger.Trace($"Entering...");
        
            StringBuilder sb = new ("SELECT ControlNo FROM OmniIntegration_ControlNumbers WHERE ControlNo = @ControlNo");
            DynamicParameters dp = new();
            dp.Add("@ControlNo", value);
        
            try
            {
                using MySqlConnection conn = new (_connectionStr);
                conn.Open();
                conn.ChangeDatabase(dataEnvironmentName);
                string controlNo = conn.Query<string>(sb.ToString(), dp).FirstOrDefault() ?? string.Empty;
                _logger.Trace($"Finished Successfully...SuppShipNo {controlNo}");
            
                return controlNo.IsEmpty();
            }
            catch (Exception e)
            {
                _logger.Error($"Error fetching Parameter for ControlNo");
                _logger.Error(e.Message);
            }

            return false;
        }
    
        /// <inheritdoc cref="IMySqlMillProvider.AddControlNo"/>
        public bool AddControlNo(string connectionStr, string controlNo, string suppShipNo, string dataEnvironmentName)
        {
            _logger.Trace($"Entering...");
        
            StringBuilder sb = new ();
            sb.AppendLine("INSERT INTO OmniIntegration_ControlNumbers ");
            sb.AppendLine("(SuppShipNo, ControlNo) ");
            sb.AppendLine(" VALUES (@SuppShipNo, @ControlNo)");
            DynamicParameters dp = new();
            dp.Add("@ControlNo", controlNo);
            dp.Add("@SuppShipNo", suppShipNo);
            try
            {
                using MySqlConnection conn = new (connectionStr);
                conn.Open();
                conn.ChangeDatabase(dataEnvironmentName);
            
                int rowsAffected = conn.Execute(sb.ToString(), dp);
                _logger.Trace($"Finished Successfully...ControlNo {controlNo} updated with SuppShipNo {suppShipNo}.  Rows Affected: {rowsAffected}");
                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Error inserting Parameter for ControlNo");
                _logger.Error(e.Message);
            }

            return false;
        }
    }
}