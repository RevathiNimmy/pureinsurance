namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimDetailsQueryBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public bool ExclusiveLock { get; set; }
        public int FetchAllVersionAmounts { get; set; }
        public bool IsRoundingUpToFour { get; set; }
        public string SessionValue { get; set; }
        public int SoureceId { get; set; }
    }
}
