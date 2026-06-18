namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCheckUnpaidPremiumResponseTypeRow
    {
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("BranchDescription")]
        public string BranchDescription { get; set; }
        //[DBCol("amount")]
        public decimal amount { get; set; }
        //[DBCol("document_date")]
        public System.DateTime document_date { get; set; }
        //[DBCol("document_ref")]
        public string document_ref { get; set; }
        //[DBCol("document_type")]
        public string document_type { get; set; }
        //[DBCol("outstanding")]
        public decimal outstanding { get; set; }
        //[DBCol("short_code")]
        public string short_code { get; set; }
    }
}
