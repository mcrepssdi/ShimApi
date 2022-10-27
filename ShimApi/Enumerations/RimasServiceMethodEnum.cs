using System.ComponentModel;

namespace ShimApi.Enumerations
{
    public enum RimasServiceMethodEnum
    {
        [Description("GetMSSHIPH")] GetMsShipHdr,
        [Description("GetMSSHIPDCS")] GetMsShipDtl,
        [Description("WriteMSSHIPDCField")] WriteMsShipDtlField // Cust_Gross, Cust_Tare, Cust_Net, Cust_Amount
    }
}