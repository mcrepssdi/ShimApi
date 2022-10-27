using System.Net;
using System.Text.Json;
using NLog;
using SdiHttpLib;
using SdiUtility;
using ShimApi.Models;
using ShimApi.Models.ResponseModels;
using ShimApi.Models.SalesShipmentModels;
using ShimApi.Services.SalesShipment;
using ShimApi.Utilities;

namespace ShimApi.Services
{
    public class SaiShipmentService : IShipmentService<SaiShipmentService>

    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ApiParameters _apiParameters;
        private readonly List<(string key, string value)> _saiPostHeaders;
        private readonly IHttp _http;
    
        public SaiShipmentService(ApiProperties apiProperties)

        {
            _apiParameters = apiProperties.ApiParameters;
            _saiPostHeaders = new List<(string key, string value)>
                {new ValueTuple<string, string>("API-Token", _apiParameters.Token)};
            _http = new SdiHttp(Logger);
        }


        /// <inheritdoc cref="IShipmentService{T}.CreateUnloadWeights"/>
        public ResponseWrapper CreateUnloadWeights(UnloadWeightsCreate unloadWeightsCreate)
        {
            Logger.Trace("Entering...");
            string json = JsonSerializer.Serialize(unloadWeightsCreate);
            Logger.Trace($"POSTing Data: {json}");
        
            string url = _apiParameters.EndPoint(SalesShipmentServiceMethod.CreateUnloadWeights.GetDescription());
            Logger.Info($"POSTing to URL: {url}");
            Task<(HttpStatusCode httpStatusCode, string responseText)>? response =
                _http.SaiPost(json, url, _saiPostHeaders);

            return response.ServiceResponseBuilder();
        }
    
        /// <inheritdoc cref="IShipmentService{T}.Validate"/>
        public ResponseWrapper Validate(ServiceLocator serviceLocator)

        {
            Logger.Trace("Entering...");
        
            ShipmentPoster shipmentPoster = (ShipmentPoster) serviceLocator;
            string json = JsonSerializer.Serialize(shipmentPoster);
            Logger.Trace($"POSTing Data: {json}");
        
            string url = serviceLocator.EndPoint(SalesShipmentServiceMethod.Validate.GetDescription());
            Logger.Trace($"POSTing to URL: {url}");
        
            Task<(HttpStatusCode httpStatusCode, string responseText)>? response =
                _http.SaiPost(json, url,
                    new List<(string key, string value)>
                        {new ValueTuple<string, string>("API-Token", serviceLocator.ApiToken)});
        
            return response.ServiceResponseBuilder();
        }
    }
}
