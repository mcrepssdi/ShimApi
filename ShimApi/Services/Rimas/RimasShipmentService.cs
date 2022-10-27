using System.Net;
using Newtonsoft.Json;
using NLog;
using SdiHttpLib;
using SdiHttpLib.Utilities;
using SdiUtility;
using ShimApi.Enumerations;
using ShimApi.Models;
using ShimApi.Models.ResponseModels;
using ShimApi.Models.Rimas;
using ShimApi.Models.SalesShipmentModels;
using ShimApi.ShimConfigMgr.Sections;

namespace ShimApi.Services.Rimas
{
    public class RimasShipmentService : IShipmentService<RimasShipmentService>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly RimasApi _rimasApiParams;
        private readonly IHttp _http = new SdiHttp(Logger);
        private string _url = string.Empty;
        private readonly ApiParameters _apiParameters;
    
        public RimasShipmentService(ApiProperties apiProperties)
        {
            _apiParameters = apiProperties.ApiParameters;
            _rimasApiParams = apiProperties.RimasApi;
        }

        public ResponseWrapper CreateUnloadWeights(UnloadWeightsCreate unloadWeightsCreate)
        {
            Logger.Trace("Entering...");
            // WriteMSSHIPDCField     Save a value to the shipper detail record
            // Get
            // https://www.sharedlogic.com/ws/rimas.svc/WriteMSSHIPDCField?sUser=mobile&sPassword=elibom&sCompanyCode=SLG&sDivisionCode=1&sControl=202699&sSequence=1&sField=PACKAGE_COUNT&sValue=1&sLog=Lincoln
            // sField Cust_Gross, Cust_Tare, Cust_Net
            _url = _apiParameters.EndPoint(RimasServiceMethodEnum.GetMsShipDtl.GetDescription());
            new List<string>(){"Cust_Gross", "Cust_Tare", "Cust_Net"}.ForEach(field =>
            {
                MsShipByField itemToPost = new MsShipByField()
                {
                    sUser = string.Empty,
                    sPassword = string.Empty,
                    sCompanyCode = string.Empty,
                    sDivisionCode = string.Empty,
                    sSequence = string.Empty,
                    sControl = string.Empty,
                    sField = field,
                    sValue = string.Empty,
                    sLog = "Service"
                };
                Task<(HttpStatusCode httpStatusCode, string responseText)> response = _http.Get(itemToPost, _url,
                    new List<(string key, string value)>());
                //https://www.sharedlogic.com/ws/rimas.svc/WriteMSSHIPDCField?sUser=mobile&sPassword=elibom&sCompanyCode=SLG&sDivisionCode=1&sControl=202699&sSequence=1&sField=PACKAGE_COUNT&sValue=1&sLog=Lincoln
            });
        
            return new ResponseWrapper();
        }

        public ResponseWrapper Validate(ServiceLocator serviceLocator)
        {
            Logger.Trace("Entering...");

            ShipmentPoster shipmentPoster = (ShipmentPoster) serviceLocator;
            Logger.Trace($"ShipmentPoster: {JsonConvert.SerializeObject(shipmentPoster)}");
        
            _url = _rimasApiParams.EndPoint(RimasServiceMethodEnum.GetMsShipDtl.GetDescription());
            Logger.Trace($"Redirecting to...{_url}");
        
            MsShipGet data = new()
            {
                sControl = shipmentPoster.ShipmentNo, 
                sPassword = _rimasApiParams.RimasPwd, 
                sUser = _rimasApiParams.RimasUser,
                sCompanyCode = _rimasApiParams.Company, 
                sDivisionCode = _rimasApiParams.Division
            };
            Logger.Trace($"RIMAS Data: {JsonConvert.SerializeObject(data)}");

            Task<(HttpStatusCode httpStatusCode, string responseText)> response = _http.Get(data, _url,
                new List<(string key, string value)>());
        
            Logger.Trace($"Response: {JsonConvert.SerializeObject(response.Result.responseText)}");
            return ValidateGetMsShipDtlResponse(response, shipmentPoster.ShipmentNo);
        }

        private static ResponseWrapper ValidateGetMsShipDtlResponse(Task<(HttpStatusCode httpStatusCode, string responseText)> response, string controlNo)
        {
            Logger.Trace("Entering...");
        
            // Control # not found
            if (response.Result.httpStatusCode != HttpStatusCode.OK || response.Result.responseText.Equals(string.Empty))
            {
                return new ResponseWrapper
                {
                    Response = new Response
                    {
                        Status = "ERROR",
                        Errors = new List<Error>
                        {
                            new(message: $"Control Number {controlNo} not found", field: "Control")
                        },
                        Message = "Errors found.",
                        Data = null
                    }
                };
            }

            // Control # Not found
            MsShipDtlResponse dtls = response.Result.responseText.CastReponse<MsShipDtlResponse>() ?? new MsShipDtlResponse();
            if (!dtls.MsShipDtls.Any())
            {
                return new ResponseWrapper
                {
                    Response = new Response
                    {
                        Status = "ERROR",
                        Errors = new List<Error>
                        {
                            new(message: $"Control Number {controlNo} not found", field: "Control")
                        },
                        Message = "Errors found.",
                        Data = null
                    }
                };
            }

            // Control # Has been unloaded
            bool unloaded =
                (from s in dtls.MsShipDtls where s.Cust_gross != 0 || s.Cust_tare != 0 || s.Cust_net != 0 select true)
                .FirstOrDefault();
            if (unloaded)
            {
                return new ResponseWrapper
                {
                    Response = new Response
                    {
                        Status = "ERROR",
                        Errors = new List<Error>
                        {
                            new(message: $"Control Number {controlNo} not found", field: "Control")
                        },
                        Message = "Errors found.",
                        Data = null
                    }
                };
            }
        
            // Found
            return new ResponseWrapper
            {
                Response = new Response
                {
                    Status = "OK",
                    Errors = new List<Error>(),
                    Message = string.Empty,
                    Data = JsonConvert.DeserializeObject(response.Result.responseText)
                }
            };
        }
    }
}
