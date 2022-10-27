using System.ComponentModel;

namespace ShimApi.Services.SalesShipment
{
    public enum LoadStatus
    {
        [Description("HOLD")]
        Hold,
        [Description("APPROVE")]
        Approve,
        [Description("REJECT")]
        Reject
    }
}
