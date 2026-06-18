namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRiskLinksQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int RiskKey { get; set; }
    }
}
