// ReSharper disable InconsistentNaming
namespace ShimApi.Models.Rimas
{
    public class MsShipDtl
    {
        public string Company { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Control { get; set; } = string.Empty;
        public string Line_type { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public string? Account { get; set; } = string.Empty;
        public string? Contract { get; set; } = string.Empty;
        public int? Contract_item { get; set; }
        public string? Tag_number { get; set; } = string.Empty;
        public string? Commodity { get; set; } = string.Empty;
        public string? Yard { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double? Ship_gross { get; set; }
        public double? Ship_tare { get; set; }
        public double? Ship_net { get; set; }
        public double? Ship_price { get; set; }
        public string? Ship_um { get; set; }
        public double? Ship_amount { get; set; }
        public double? Ship_recover { get; set; }
        public double? Cust_gross { get; set; }
        public double? Cust_tare { get; set; }
        public double? Cust_net { get; set; }
        public double? Cust_price { get; set; }
        public string? Cust_um { get; set; } = string.Empty;
        public double? Cust_amount { get; set; }
        public double? Cust_recover { get; set; }
        public double? Setl_gross { get; set; }
        public double? Setl_tare { get; set; }
        public double? Setl_net { get; set; }
        public double? Setl_price { get; set; }
        public string? Setl_um { get; set; } = string.Empty;
        public double? Setl_amount { get; set; }
        public double? Setl_recover { get; set; }
        public string? Post_type { get; set; } = string.Empty;
        public string? Comm_type { get; set; } = string.Empty;
        public string? Packaging { get; set; } = string.Empty;
        public int? Package_count { get; set; }
        public string? Weight_code { get; set; } = string.Empty;
        public string? Tax_code { get; set; } = string.Empty;
        public double? Tax_amount { get; set; }
        public double? Frt_exp_amount { get; set; }
        public double? Cost_amount { get; set; }
        public string? Comment_print { get; set; } = string.Empty;
        public string? Comment { get; set; } = string.Empty;
        public DateTime? Setl_date { get; set; }
        public double? Weight_adj { get; set; }
        public double? Price_adj { get; set; }
        public double? Freight_adj { get; set; }
        public double? Process_adj { get; set; }
        public double? Misc_adj { get; set; }
        public double? Pay_amount { get; set; }
        public string? Posted { get; set; } = string.Empty;
        public double? Homecurr_setl { get; set; }
        public double? Homecurr_frt { get; set; }
        public double? Homecurr_cost { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Createtime { get; set; }
        public string? Createuser { get; set; } = string.Empty;
        public string? Createstation { get; set; } = string.Empty;
        public DateTime? Lastdate { get; set; }
        public DateTime? Lasttime { get; set; }
        public string? Lastuser { get; set; } = string.Empty;
        public string? Laststation { get; set; } = string.Empty;
        public string? Ship_gross_manual { get; set; } = string.Empty;
        public string? Ship_tare_manual { get; set; } = string.Empty;
        public double? Material_cost { get; set; }
        public double? Freight_cost { get; set; }
        public double? Processing_cost { get; set; }
        public double? Out_proc_cost { get; set; }
        public DateTime? Ship_gross_time { get; set; }
        public DateTime? Ship_tare_time { get; set; }
        public double? Packaging_cost { get; set; }
        public DateTime? Admin_timestamp { get; set; }
        public string? Proforma_invoice { get; set; } = string.Empty;
        public double? Weight_adj_net { get; set; }
        public double? Weight_adj_price { get; set; }
        public string? Weight_adj_comment { get; set; } = string.Empty;
        public double? Price_adj_net { get; set; }
        public double? Price_adj_price { get; set; }
        public string? Price_adj_comment { get; set; } = string.Empty;
        public double? Freight_adj_net { get; set; }
        public double? Freight_adj_price { get; set; }
        public string? Freight_adj_comment { get; set; } = string.Empty;
        public double? Process_adj_net { get; set; }
        public double? Process_adj_price { get; set; }
        public string? Process_adj_comment { get; set; } = string.Empty;
        public double? Misc_adj_net { get; set; }
        public double? Misc_adj_price { get; set; }
        public string? Misc_adj_comment { get; set; } = string.Empty;
        public double? Misc2_adj { get; set; }
        public double? Misc2_adj_net { get; set; }
        public double? Misc2_adj_price { get; set; }
        public string? Misc2_adj_comment { get; set; } = string.Empty;
        public string? Weight_um { get; set; } = string.Empty;
        public DateTime? Recon_datetime { get; set; }
        public string? Recon_user { get; set; } = string.Empty;
        public string? Recon_posted_ci { get; set; } = string.Empty;
        public string? Recon_commodity { get; set; } = string.Empty;
        public string? Recon_yard { get; set; }
        public DateTime? Recon_ship_date { get; set; }
        public double? Recon_net { get; set; }
        public double? Recon_amount { get; set; }
        public string? Recon_post_type { get; set; } = string.Empty;
        public string? Recon_inventory_wgt { get; set; } = string.Empty;
        public string? Sfdc_id { get; set; } = string.Empty;
        public DateTime? Sfdc_sync_create_date { get; set; }
    }
}