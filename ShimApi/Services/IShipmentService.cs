using ShimApi.Models;
using ShimApi.Models.ResponseModels;
using ShimApi.Models.SalesShipmentModels;

namespace ShimApi.Services
{
    public interface IShipmentService<T>

    {
        /// <summary>
        /// Calls the SAI/RIMAS Endpoint CreateUnloadWeights
        /// </summary>
        /// <param name="unloadWeightsCreate"></param>
        /// <returns>ResponseWrapper</returns>
        public ResponseWrapper CreateUnloadWeights(UnloadWeightsCreate unloadWeightsCreate);
    
        /// <summary>
        /// Calls the SAI/RIMAS Endpoint Validate
        /// </summary>
        /// <param name="serviceLocator"></param>
        /// <returns>ResponseWrapper</returns>
        public ResponseWrapper Validate(ServiceLocator serviceLocator);
    }
}
