namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductByAgentResponseTypeRow
    {
        //[DBCol("block_no")]
        public string BlockNumber { get; set; }

        //[DBCol("lead_allow_consolidated_commission")]
        public bool ConsolidatedLeadAgentCommission { get; set; }

        //[DBCol("sub_allow_consolidated_commission")]
        public bool ConsolidatedSubAgentCommission { get; set; }

        //[DBCol("caption")]
        public string Description { get; set; }

        //[DBCol("code")]
        public string ProductCode { get; set; }

        //[DBCol("product_id")]
        public int ProductKey { get; set; }

        //[DBCol("scheme_agency_ref")]
        public string SchemeAgencyRef { get; set; }
    }
}
