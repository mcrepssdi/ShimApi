using System.ComponentModel;

namespace ShimApi.Services.SalesShipment
{
    public enum SalesShipmentServiceMethod
    {
        Default,

        /* Sales Shipment API Service Methods */
        [Description("SalesShipment/CreateUnloadWeights")]
        CreateUnloadWeights,
        [Description("SalesShipment/Validate")]
        Validate,
    }
}
