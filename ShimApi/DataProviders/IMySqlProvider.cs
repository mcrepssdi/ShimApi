using NLog;
using ShimApi.Enumerations;
using ShimApi.Models;

namespace ShimApi.DataProviders
{
    public interface IMySqlProvider
    {
        /// <summary>
        /// Fetches the required information to make a POST to the SAI API.
        /// </summary>
        /// <param name="schema">>Database schema</param>
        /// <param name="serviceName"></param>
        /// <param name="serviceMethod"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        (SqlState sqlState, string token) SaiServiceToken(string schema, string serviceName = "*", string serviceMethod = "*", Logger? logger = null);

        /// <summary>
        /// Fetches the required information to make a POST to the SAI API.
        /// </summary>
        /// <param name="schema">>Database schema</param>
        /// <param name="serviceName"></param>
        /// <param name="serviceMethod"></param>
        /// <param name="branch"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        (SqlState sqlState, SaiServiceLocator serviceLocator) SaiServiceLocator(string schema, string serviceName = "*", string serviceMethod = "*", string branch = "*",Logger? logger = null);
    }
}