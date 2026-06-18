namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReinsuranceArrangementLinesQueryBase : BaseRequestType
    {
        public int ArrangementId { get; set; }
        public int ClaimId { get; set; }
        public int Mode { get; set; }
        public bool ModeSpecified { get; set; }
    }
}
