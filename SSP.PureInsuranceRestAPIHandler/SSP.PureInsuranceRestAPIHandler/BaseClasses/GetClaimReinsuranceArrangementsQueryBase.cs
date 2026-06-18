namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReinsuranceArrangementsQueryBase : BaseRequestType
    {
        public int ClaimId { get; set; }
        public int Mode { get; set; }
        public bool ModeSpecified { get; set; }
    }
}
