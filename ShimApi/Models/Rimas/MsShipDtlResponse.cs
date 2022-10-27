
using Newtonsoft.Json;

namespace ShimApi.Models.Rimas
{
    public class MsShipDtlResponse
    {
        [JsonProperty(PropertyName = "Dataset")]
        public List<MsShipDtl> MsShipDtls { get; set; } = new();
    }
}