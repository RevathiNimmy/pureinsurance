namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyDiscountInfoQuery : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }

    public class GetPolicyDiscountInfoQueryResponse
    {
        public bool IsDiscountApplied { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedPremium { get; set; }
        public int DiscountReasonId { get; set; }
        public int MatchDiscountedPremium { get; set; }
        public int RecurringTypeId { get; set; }
        public decimal TotalPremium { get; set; }
    }
}
