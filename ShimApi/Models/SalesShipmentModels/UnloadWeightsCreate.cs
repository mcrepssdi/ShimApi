namespace ShimApi.Models.SalesShipmentModels
{
    public class UnloadWeightsCreate : ServiceAware
    {
        public string DataEnvironmentName { get; init; } = string.Empty;
        public SalesShipmentUnloadWeights UnloadWeights { get; set; } = new();
    }
}
