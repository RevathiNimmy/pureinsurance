namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPoliciesForRenewalSelectionResponseTypeRow
    {
        //[DBCol("anniversary_copy")]
        public bool AnniversaryCopy { get; set; }
        //[DBCol("client_name")]
        public string Client { get; set; }
        //[DBCol("client_code")]
        public string ClientCode { get; set; }
        //[DBCol("expiry_date")]
        public System.DateTime CoverEndDate { get; set; }
        //[DBCol("cover_start_date")]
        public System.DateTime CoverStartDate { get; set; }
        //[DBCol("insurance_file_cnt")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("Insurance_ref")]
        public string InsuranceFileRef { get; set; }
        //[DBCol("insurance_folder_cnt")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("is_auto_renewable")]
        public bool IsAutoRenewable { get; set; }
        //[DBCol("is_deleted")]
        public bool IsClosed { get; set; }
        //[DBCol("is_in_transfer_mode")]
        public bool IsInTransferMode { get; set; }
        //[DBCol("is_true_monthly_policy")]
        public bool IsTrueMonthlyPolicy { get; set; }
        //[DBCol("agent_name")]
        public string LeadAgent { get; set; }
        //[DBCol("lead_agent_cnt")]
        public int LeadAgentKey { get; set; }
        //[DBCol("insurance_holder_cnt")]
        public int PartyKey { get; set; }
        //[DBCol("Product_description")]
        public string ProductDescription { get; set; }
        //[DBCol("product_id")]
        public int ProductKey { get; set; }
        //[DBCol("renewal_count")]
        public int RenewalCount { get; set; }
        //[DBCol("renewal_date")]
        public System.DateTime RenewalDate { get; set; }
    }
}
