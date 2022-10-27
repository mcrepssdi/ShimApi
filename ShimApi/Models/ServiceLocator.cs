using System.Text.Json.Serialization;

namespace ShimApi.Models
{
    public class ServiceLocator
    {
        [JsonIgnore]
        public string TargetUrl { get; set; } = string.Empty;
        [JsonIgnore]
        public string ApiToken { get; set; } = string.Empty;
    
        public string EndPoint(string method)
        {
            Uri uri = new ($"{TargetUrl}/{method}");
            return uri.AbsoluteUri;
        }
    }
}