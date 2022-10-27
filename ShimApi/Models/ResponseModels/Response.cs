using Newtonsoft.Json;

namespace ShimApi.Models.ResponseModels
{
    public class Response
    {
        [JsonProperty("status")] 
        public string Status { get; set; } = string.Empty;
    
        [JsonProperty("message")]
        public string Message { get; set; }= string.Empty;

        [JsonProperty("errors")] 
        public List<Error> Errors { get; set; } = new();

        [JsonProperty("data")] 
        public object? Data { get; set; }
    
        [JsonProperty("Carrier")] 
        public object? Carrier { get; set; }
    
        [JsonProperty("Addresses")] 
        public object? Addresses { get; set; }
    }
}