using Newtonsoft.Json;

namespace ShimApi.Models.ResponseModels
{
    public class Error
    {
        public Error()
        {
        }

        public Error(string message, string field)
        {
            Message = message;
            Field = field;
        }

        [JsonProperty("field")] 
        public string Field { get; set; } = string.Empty;
    
        [JsonProperty("message")]
        public string Message { get; set; }= string.Empty;
    }
}