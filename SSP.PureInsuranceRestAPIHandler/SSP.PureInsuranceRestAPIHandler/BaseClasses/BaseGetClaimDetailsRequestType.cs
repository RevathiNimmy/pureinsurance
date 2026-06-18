namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimDetailsRequestType : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public bool ExclusiveLock { get; set; }
        public int FetchAllVersionAmounts { get; set; }
        public bool IsRoundingUpToFour { get; set; }
        public string SessionValue { get; set; }

        public int SourceId { get; set; }
    }
}