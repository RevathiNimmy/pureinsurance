namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRecoveryTransactionsQuery
    {
        public string LoginUserName { get; set; }
        public string Route { get; set; }
        public string BranchCode { get; set; }
        public string ShortCode { get; set; }
        public string ClaimNumber { get; set; }
    }
}
