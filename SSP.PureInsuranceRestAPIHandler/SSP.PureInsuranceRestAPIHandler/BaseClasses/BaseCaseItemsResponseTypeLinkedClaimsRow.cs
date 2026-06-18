namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCaseItemsResponseTypeLinkedClaimsRow
    {
        //[DBCol("Claim Handler")]
        public string ClaimHandler { get; set; }
        //[DBCol("claim_id")]
        public int ClaimKey { get; set; }
        //[DBCol("claim_number")]
        public string ClaimNumber { get; set; }
        //[DBCol("policy_id")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("loss_date")]
        public System.DateTime LossDate { get; set; }
        //[DBCol("Risk Type")]
        public string RiskType { get; set; }
        //[DBCol("status")]
        public string Status { get; set; }
        //[DBCol("total_excess")]
        public decimal TotalExcess { get; set; }
        //[DBCol("total_expense")]
        public decimal TotalExpense { get; set; }
        //[DBCol("total_indemnity")]
        public decimal TotalIdmenity { get; set; }
    }
}
