namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRiskReadOnlyQueryBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public int BaseClaimKey { get; set; }
        public bool IgnoreIsDirty { get; set; }
    }
}
