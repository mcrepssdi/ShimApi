using System.Net;
using System.Text.Json;
using NLog;
using SdiHttpLib;
using SdiHttpLib.Utilities;
using ShimApi.Models;
using ShimApi.Models.ResponseModels;

namespace ShimApi.Services
{
    public class ApiService : IApiService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ApiParameters _apiParameters;
        private readonly List<(string key, string value)> _saiPostHeaders;
        private readonly IHttp _http;

        public ApiService(ApiProperties apiProperties)
        {
            _apiParameters = apiProperties.ApiParameters;
            _saiPostHeaders = new List<(string key, string value)> { new ValueTuple<string, string>("API-Token", _apiParameters.Token) };
            _http = new SdiHttp(Logger);
        }

        /// <inheritdoc cref="IApiService.PassThroughCalls"/>
        public ResponseWrapper? PassThroughCalls(object data, HttpRequest request)
        {
            Logger.Trace("Entering...");
            string json = JsonSerializer.Serialize(data);
            string path = request.Path.Value?.Replace("/shim/", "") ?? string.Empty;
            string url = _apiParameters.EndPoint(path);
        
            Task<(HttpStatusCode httpStatusCode, string responseText)>? response =
                _http.SaiPost(json, url, _saiPostHeaders);

            return response.CastReponse<ResponseWrapper?>();
        }
    }
}
