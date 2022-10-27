namespace ShimApi.Models.SalesShipmentModels
{
    public class ShipmentPoster : ServiceLocator
    {
        public string DataEnvironmentName { get; init; } = string.Empty;
        public string ShipmentNo { get; set; } = string.Empty;
        public string CustomerNo { get; init; } = string.Empty;
        public string PONo { get; init; } = string.Empty; // Customer PONo from the Sales Order Header
        public string POLine { get; init; } = string.Empty;
    }
}