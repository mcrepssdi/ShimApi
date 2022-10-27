namespace ShimApi.Models.SalesShipmentModels
{
    public class SalesShipmentUnloadWeights
    {
        public string CustomerNo { get; set; } = string.Empty;
        public string ShipmentNo { get; set; } = string.Empty;
        public string CustomerShipRefNo { get; set; } = string.Empty;                               // Optional
        public string UnloadDate { get; set; } = DateTime.MinValue.ToString("yyyy-MM-dd");      // UnloadDate >= ShipmentDate
        public int GrossWt { get; set;} = 0;                                                        // GrossWt > 0 & GrossWt - TareWt > 0
        public int TareWt { get; set; } = 0;                                                        // TareWt > 0 & GrossWt - TareWt > 0
        public string WtToleranceHandling { get; set; } = "HOLD";
        public string PriceUM { get; set; } = string.Empty;                                 // Required if Price is included.
        public decimal Price { get; set; } = 0;                                             // Optional. Price > 0.0000
        public string PriceMismatchHandling { get; set; } = "HOLD";                         // Required if Price and PriceUm are included.
        public string RegisterPrintFlag { get; set; } = "Y";                                // Optional
        public string LoadComment { get; set; } = string.Empty;                             // Optional
        public string Username { get; set; } = "service";                                   // Optional
    }
}
