namespace ShimApi.Models.SalesShipmentModels
{
    public class UnloadWeightPoster: ServiceLocator
    {
        public string DataEnvironmentName { get; init; } = string.Empty;
        public SalesShipmentUnloadWeights UnloadWeights { get; set; } = new();
    }
}