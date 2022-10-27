using Dapper;
using MySql.Data.MySqlClient;
using NLog;
using ShimApi.Enumerations;
using ShimApi.Models;

namespace ShimApi.DataProviders
{
    public class SaiProvider : IMySqlProvider
    {
        private readonly string _connectionStr;
        private readonly int _commandTimeout;
    
        public SaiProvider(string connstr, int commandTimeout = 120)
        {
            _connectionStr = connstr;
            _commandTimeout = commandTimeout;
        }

        /// <inheritdoc cref="IMySqlProvider.SaiServiceToken"/>
        public (SqlState sqlState, string token) SaiServiceToken(string schema, string serviceName = "*", string serviceMethod = "*", Logger? logger = null)
        {
            logger ??= LogManager.CreateNullLogger();
        
            string token = string.Empty;
            using MySqlConnection conn = new(_connectionStr);
            try
            {
                conn.Open();
                (string sql, DynamicParameters dp) = SaiQueryBuilder.ServiceTokenQuery(schema,serviceName,serviceMethod);
                token  = conn.QueryFirstOrDefault<string>(sql, dp);
                return (SqlState.Ok, token);
            }
            catch (Exception e)
            {
                logger.Error($"SQL Exception fetching API Params.\r\n\t\tMsg: {e.Message}");
            }
            return (SqlState.Failed, token);
        }

        /// <inheritdoc cref="IMySqlProvider.SaiServiceLocator"/>
        public (SqlState sqlState, SaiServiceLocator serviceLocator) SaiServiceLocator(string schema, string serviceName = "*",
            string serviceMethod = "*", string branch = "*", Logger? logger = null)
        {
            logger ??= LogManager.CreateNullLogger();
        
            using MySqlConnection conn = new(_connectionStr);
            try
            {
                conn.Open();
                (string sql, DynamicParameters dp) = SaiQueryBuilder.InternalServerPathQuery(schema,serviceName,serviceMethod,branch);
                IEnumerable<SaiWebserviceProp>? rows = conn.Query<SaiWebserviceProp>(sql, dp);
                return (SqlState.Ok, new SaiServiceLocator(rows));
            }
            catch (Exception e)
            {
                logger.Error($"SQL Exception fetching API Params.\r\n\t\tMsg: {e.Message}");
            }
            return (SqlState.Failed, new SaiServiceLocator());
        }
    }
}