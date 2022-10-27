using ShimApi.Models.ResponseModels;

namespace ShimApi.Services
{
    public interface IApiService
    {
        /// <summary>
        /// This method is a default endpoint that will pass the information to the correct API.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="request"></param>
        public ResponseWrapper? PassThroughCalls(object data, HttpRequest request);
    }
}
