using Newtonsoft.Json;

namespace ShimApi.Models.ResponseModels
{
    public class ResponseWrapper
    {
        [JsonProperty("response")] 
        public Response Response { get; set; } = new();
    }
}