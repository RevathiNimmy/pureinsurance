namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ProcessClaimCommandBase : BaseRequestType
    {
        public BaseClaimProcessType Claim { get; set; }
        public bool ExclusiveLock { get; set; }
        public bool IsMaintainClaim { get; set; }
        public string SessionValue { get; set; }
        public string ClaimNumber { get; set; }
        public int ClaimKey { get; set; }
        public bool IsPaymentRequired { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public int BaseClaimKey { get; set; }
        public string ClaimRiskXML { get; set; }
        public int ClaimVersion { get; set; }
        public int SourceId { get; set; }
        public BaseGetClaimDetailsType ClaimDetails { get; set; }
    }
}
