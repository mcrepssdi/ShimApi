using System.Net;
using System.Text.RegularExpressions;
using SdiHttpLib.Utilities;
using SdiPasswordLib;
using SdiUtility;
using ShimApi.DataProviders;
using ShimApi.Enumerations;
using ShimApi.Models;
using ShimApi.Models.ResponseModels;
using ShimApi.Models.SalesShipmentModels;

namespace ShimApi.Utilities
{
    public static class ApiUtil
    {
        public static SoftwareVendor GoToSoftware(string shipmentNo)
        {
            const string pattern = @"\d{1,1}";
            return shipmentNo.Length == 6 && Regex.IsMatch(shipmentNo[..1], pattern)
                ? SoftwareVendor.Sai
                : SoftwareVendor.Rimas;
        }
    
        public static ServiceLocator UnloadWeightsServiceLocator(this ServiceAware serviceAware, IMySqlMillProvider saiProvider)
        {
            UnloadWeightsCreate unloadWeightsCreate = (UnloadWeightsCreate) serviceAware;
            ServiceLocator serviceLocator = new UnloadWeightPoster
            {
                DataEnvironmentName = unloadWeightsCreate.DataEnvironmentName,
                UnloadWeights  = new SalesShipmentUnloadWeights()
                {
                    CustomerNo = unloadWeightsCreate.UnloadWeights.CustomerNo,
                    ShipmentNo = unloadWeightsCreate.UnloadWeights.ShipmentNo,
                    CustomerShipRefNo = unloadWeightsCreate.UnloadWeights.CustomerShipRefNo,
                    UnloadDate = unloadWeightsCreate.UnloadWeights.UnloadDate,
                    GrossWt = unloadWeightsCreate.UnloadWeights.GrossWt,
                    TareWt = unloadWeightsCreate.UnloadWeights.TareWt,
                    WtToleranceHandling = unloadWeightsCreate.UnloadWeights.WtToleranceHandling,
                    PriceUM = unloadWeightsCreate.UnloadWeights.PriceUM,
                    Price = unloadWeightsCreate.UnloadWeights.Price,
                    PriceMismatchHandling = unloadWeightsCreate.UnloadWeights.PriceMismatchHandling,
                    RegisterPrintFlag = unloadWeightsCreate.UnloadWeights.RegisterPrintFlag,
                    LoadComment = unloadWeightsCreate.UnloadWeights.LoadComment,
                    Username = "service"
                }
            };
            return serviceLocator;
        }
    
        public static ServiceLocator TruckScalePost(this ServiceAware serviceAware, IMySqlMillProvider saiProvider)
        {
            ValidatePost validatePost = (ValidatePost) serviceAware;
            string dataEnvironment = saiProvider.WebServicePropertyHierarchical(serviceAware,"Environment");
            string customerNo = saiProvider.WebServicePropertyHierarchical(serviceAware,"CustomerNo");
            string apiToken = saiProvider.WebServicePropertyHierarchical(serviceAware,"APIToken");
            string targerUrl = saiProvider.WebServicePropertyHierarchical(serviceAware,"TargetURL");

            ServiceLocator serviceLocator = new ShipmentPoster
            {
                DataEnvironmentName = dataEnvironment,
                CustomerNo = customerNo,
                PONo = validatePost.PONo,
                POLine = validatePost.POLine,
                ShipmentNo = validatePost.ShipmentRefNo
            };
            serviceLocator.TargetUrl = targerUrl;
            serviceLocator.ApiToken = apiToken;
            return serviceLocator;
        }
    
        public static ServiceLocator AllOtherPost(this ServiceAware serviceAware, IMySqlMillProvider saiProvider)
        {
            ValidatePost validatePost = (ValidatePost) serviceAware;
        
            string dataEnvironment = saiProvider.WebServicePropertyHierarchical(serviceAware,"Environment");
            string apiToken = saiProvider.WebServicePropertyHierarchical(serviceAware,"APIToken");
            string targerUrl = saiProvider.WebServicePropertyHierarchical(serviceAware,"TargetURL");
        
            ServiceLocator serviceLocator = new ShipmentPoster
            {
                DataEnvironmentName = dataEnvironment,
                CustomerNo = validatePost.CustomerNo,
                PONo = validatePost.PONo,
                POLine = validatePost.POLine,
                ShipmentNo = validatePost.ShipmentNo
            };
            serviceLocator.TargetUrl = targerUrl;
            serviceLocator.ApiToken = apiToken;
            return serviceLocator;
        }

        public static string GetResponseStatus(this Response value)
        {
            return value.Status switch
            {
                "SUCCESS" => value.Status,
                "OK" => value.Status,
                "SYSTEM_ERROR" => value.Status,
                _ => "INVALID"
            };
        }

        public static ResponseWrapper ServiceResponseBuilder(this Task<(HttpStatusCode httpStatusCode, string responseText)>? response)
        {
            if (response is null)
            {
                return new ResponseWrapper
                {Response = new Response
                {
                    Status = "STSTEM_ERROR", 
                    Errors = new List<Error> { new(){Field = "", Message = "No response from service"}}
                }};
            }
        
            return response.Result.responseText.Contains("response") switch
            {
                true => response.CastReponse<ResponseWrapper?>() ?? new ResponseWrapper
                    {Response = new Response {Status = "STSTEM_ERROR"}},
                _ => new ResponseWrapper
                {
                    Response = response.CastReponse<Response?>() ?? new Response {Status = "STSTEM_ERROR"}
                }
            };
        }
        
        public static string GetPassword(this string pwd)
        {
            if (pwd is null) throw new ArgumentNullException(pwd);
            if (pwd.Trim().IsEmpty()) throw new ArgumentNullException(pwd);
        
            try
            {
                string Password = Encryption.Unprotect(pwd);
                return Password;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
