using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using SdiUtility;
using ShimApi.DataProviders;
using ShimApi.Enumerations;
using ShimApi.Models;
using ShimApi.Models.ResponseModels;
using ShimApi.Models.SalesShipmentModels;
using ShimApi.Services;
using ShimApi.Services.Rimas;
using ShimApi.ShimConfigMgr;
using ShimApi.Utilities;


namespace ShimService.API.Controllers
{
    [ApiController]
    [Route("/shim")]
    [Route("/shim/BrokerageShipment")]
    [Route("/shim/Customer")]
    [Route("/shim/InboundTransfer")]
    [Route("/shim/InboundWeight")]
    [Route("/shim/Journal")]
    [Route("/shim/NonContractFreight")]
    [Route("/shim/Payment")]
    [Route("/shim/Order")]
    [Route("/shim/Production")]
    [Route("/shim/PurchaseOrder")]
    [Route("/shim/PurchaseShipment")]
    [Route("/shim/SalesOrder")]
    [Route("/shim/SalesScheduling")]
    //[Route("/shim/SalesShipment")]
    [Route("/shim/SalesTicket")]
    [Route("/shim/Supplier")]
    public class ShimApiController : ControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IApiService _apiService;
        private readonly IShipmentService<SaiShipmentService> _saiService;
        private readonly IShipmentService<RimasShipmentService> _rimasService;
        private readonly IMsSql _msSqlProvider;
        private readonly IMySqlMillProvider _saiMillProvider;
        private readonly IShimConfigurations _apiConfig;

        private const string ServiceName = "SalesShipment";
        private const string CallValidateServiceMethod = "CallValidate";

        public ShimApiController(IShimConfigurations apiConfig, IApiService apiService,
            IShipmentService<SaiShipmentService> saiService, IShipmentService<RimasShipmentService> rimasService,
            IMsSql msSqlProvider, IMySqlMillProvider saiMillProvider, ApiProperties apiProperties )
        {
            _apiService = apiService;
            _saiService = saiService;
            _rimasService = rimasService;
            _msSqlProvider = msSqlProvider;
            _saiMillProvider = saiMillProvider;
            _apiConfig = apiConfig;
        }

        [HttpGet]
        [HttpPost]
        [HttpPut]
        [Route("/{**catchAll}")]
        public ResponseWrapper Default(object data)
        {
            Logger.Trace("Entering Default...");
        
            HttpRequest request = HttpContext.Request;
            ResponseWrapper? responseToSai = _apiService.PassThroughCalls(data, request);

            return responseToSai ?? new ResponseWrapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public ResponseWrapper Echo()
        {
            Logger.Trace($"Entering Echo.  Incoming IPAddress: {LogIncomingIpAddress()}");
            VersionUtil vu = new();

            (bool connected, string response)  = _msSqlProvider.Version();
            Echo echo = new ()
            {
                CommitId = vu.FullCommitId,
                Branch = vu.GitBranch,
                Message = response,
                CanConnect = connected
            };              

            return echo.CanConnect switch
            {
                true => new ResponseWrapper
                {
                    Response = new Response
                    {
                        Status = "OK",
                        Message = "SUCCESS",
                        Data = JsonConvert.SerializeObject(echo),
                        Errors = new List<Error>()
                    }
                },
                _ => new ResponseWrapper
                {
                    Response = new Response
                    {
                        Status = "ERROR",
                        Message = "FALIED",
                        Data = string.Empty,
                        Errors = new List<Error> {new Error() {Message = JsonConvert.SerializeObject(echo)}}
                    }
                }
            };
        }
    
    
        #region Sales Shipment API Methods
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unloadWeightsCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Route("SalesShipment/[action]")]
        public ResponseWrapper CallCreateUnloadWeights(UnloadWeightsCreate unloadWeightsCreate)
        {
            Logger.Trace($"Entering CallCreateUnloadWeights.  Incoming IPAddress: {LogIncomingIpAddress()}");
            Logger.Trace($"Data Packet: {JsonConvert.SerializeObject(unloadWeightsCreate)}");
        
            ResponseWrapper response =  ApiUtil.GoToSoftware(unloadWeightsCreate.UnloadWeights.ShipmentNo) switch
            {
                SoftwareVendor.Sai => _saiService.CreateUnloadWeights(unloadWeightsCreate),
                _ => _rimasService.CreateUnloadWeights(unloadWeightsCreate)
            };
        
            response.Response.Status = response.Response.GetResponseStatus();
            Logger.Trace($"Response: {JsonConvert.SerializeObject(response)}");
        
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validatePost"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [Route("SalesShipment/[action]")]
        public ResponseWrapper CallValidate(ValidatePost validatePost)
        {
            Logger.Trace($"Entering CallValidate.  Incoming IPAddress: {LogIncomingIpAddress()}");
            Logger.Trace($"Data Packet: {JsonConvert.SerializeObject(validatePost)}");
            Logger.Trace($"Mill Working Environment: {validatePost.DataEnvironmentName}");
            string millEnvirnoment = validatePost.DataEnvironmentName;
        
            ServiceAware serviceAware = validatePost;
            serviceAware.ServiceName = ServiceName;
            serviceAware.ServiceMethod = CallValidateServiceMethod;
            Logger.Trace($"Service Call Info: {JsonConvert.SerializeObject(serviceAware)}");
        
            //This check is required becuase truckscale (TRK1) calls this method with a different signature
            bool isTruckScale = validatePost.IsTruckScalePost();
            Logger.Trace($"isTruckScale: {isTruckScale}");
            ServiceLocator serviceLocator = isTruckScale switch
            {
                true => validatePost.TruckScalePost(_saiMillProvider),
                _ => validatePost.AllOtherPost(_saiMillProvider)
            };
            ShipmentPoster poster = (ShipmentPoster) serviceLocator;
            Logger.Trace($"ShipmentPoster: {JsonConvert.SerializeObject(poster)}");

            SoftwareVendor softwareVendor = ApiUtil.GoToSoftware(poster.ShipmentNo);
            Logger.Trace($"Software Vendor: {softwareVendor}");
            ResponseWrapper response =  softwareVendor switch
            {
                SoftwareVendor.Sai => _saiService.Validate(serviceLocator),
                _ => _rimasService.Validate(serviceLocator)
            };

            //  If RIMAS We need to handle this differently, becuase of 10 Long Control #'s
            bool isResponseSuccess = IsResponseValid(response);
            Logger.Trace($"Response Status: {isResponseSuccess}");
            if (softwareVendor == SoftwareVendor.Rimas && isResponseSuccess)
            {
                SupplierShipmentRefNo suppRefNo = new(_apiConfig, _saiMillProvider);
                string rimasSuppShipNo = suppRefNo.RimasSixCharacterSuppShipNo(millEnvirnoment);
                response = SupplierShipmentRefNo.SetResponseOnRimasIdentifier(response, rimasSuppShipNo);
                response = suppRefNo.SetControllNo(response, poster.ShipmentNo, rimasSuppShipNo, millEnvirnoment);
            }
        
            /* SAI Truckscale POST */
            if (isTruckScale)
            {
                response.Response.Status = response.Response.GetResponseStatus();
                Logger.Trace($"TRK1 Response: {JsonConvert.SerializeObject(response)}");
                return response;
            }
        
            Logger.Trace($"Response: {JsonConvert.SerializeObject(response)}");
            return response;
        }
        #endregion


        private string LogIncomingIpAddress()
        {
            HttpRequest request = HttpContext.Request;
            string ipAddress = request.Headers["host"];

            return ipAddress;
        }

        private static bool IsResponseValid(ResponseWrapper response)
        {
            return response.Response.Status.Equals("OK") || response.Response.Status.Equals("SUCCESS");
        }
    
    }
}
