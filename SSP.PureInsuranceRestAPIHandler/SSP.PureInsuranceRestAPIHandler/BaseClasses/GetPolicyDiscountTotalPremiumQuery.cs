namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyDiscountTotalPremiumQuery : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }

    public class GetPolicyDiscountTotalPremiumQueryResponse
    {
        public decimal TotalPremium { get; set; }
    }
}
