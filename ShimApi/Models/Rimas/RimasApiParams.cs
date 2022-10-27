
namespace ShimApi.Models.Rimas
{
    public class RimasApiParams : ApiParameters
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
    }
}
