using SdiUtility;

namespace ShimApi.Models.SalesShipmentModels
{
    public class ValidatePost : ServiceAware
    {
        /* These are common by all calls */
        public string DataEnvironmentName { get; init; } = string.Empty;
        public string PONo { get; init; } = string.Empty; // Customer PONo from the Sales Order Header
        public string POLine { get; init; } = string.Empty;
    
    
        /* These values are supplied by TRK1 */
        public string SupplierNo { get; init; } = string.Empty;
        public string Branch { get; init; } = string.Empty;
        public string ShipmentRefNo { get; set; } = string.Empty;
    
    
    
        /* Theses are supplier by anything other call than TRK1 */
        public string CustomerNo { get; set; } = string.Empty;
        public string ShipmentNo { get; set; } = string.Empty;
    
        public bool IsTruckScalePost()
        {
            return Branch.IsNotEmpty() && ShipmentRefNo.IsNotEmpty();
        }
    }
}
