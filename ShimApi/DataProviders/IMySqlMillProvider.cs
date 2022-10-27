using ShimApi.Models;

namespace ShimApi.DataProviders
{
    public interface IMySqlMillProvider
    {
        /// <summary>
        ///    Loads the service properties for POSTing to the client server (SDI in this case)
        ///    This is an override to the standard webServicePropertyHierarchical call, as the database query was modified
        ///    to support a Value separated by a ":".  Example ZZZZB4:dl4merl80.  This is specific for a given supplier
        ///    for the 4 parameters; CustomerNo, TargetURL, Environment, APIToken
        ///
        ///    CustomerNo: Sales Order Customer for the targetUrl
        ///    TargetUrl: Server name for the specific end point.  OMNI enters 050544 as the ShipmentRefNo and this service POSTs the request to SDI
        ///     Example: http://me-spare3-P.amcs.local:8080/cresws
        ///    Environment: Target Environment where the shipment is to be validated
        ///    APIToken: Target environment API-Token*
        /// </summary>
        /// <param name="serviceAware"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public string WebServicePropertyHierarchical(ServiceAware serviceAware, string property);
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataEnvironmentName"></param>
        /// <returns></returns>
        bool IsUnique(string value, string dataEnvironmentName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <param name="controlNo"></param>
        /// <param name="suppShipNo"></param>
        /// <param name="dataEnvironmentName"></param>
        /// <returns></returns>
        bool AddControlNo(string connectionStr, string controlNo, string suppShipNo, string dataEnvironmentName);
    }
}