using SdiUtility;

namespace ShimApi.Models
{
    public class ApiParameters
    {
        public string Token { get; set; } = string.Empty;
        public string ServicePath { get; set; }= string.Empty;
        public string ServiceUrl { get; set; }= string.Empty;
    
        public string EndPoint(string method)
        {
            if (ServicePath.IsEmpty() || ServiceUrl.IsEmpty())
            {
                return "N/A";
            }
            Uri uri = new ($"http://{ServiceUrl}/{ServicePath}/{method}");
            return uri.AbsoluteUri;
        }
    }
}