namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRiskQueryBase : BaseRequestType
    {
        public int BaseClaimKey { get; set; }
        public int ClaimKey { get; set; }
        public bool IgnoreIsDirty { get; set; }
        // public byte[] ApiTimeStampKey { get; set; } = new byte[0];
        public string ApiTimeStampKey { get; set; }
        public int SourceId { get; set; }
        public int Task { get; set; }
        public bool IsDataTransfer { get; set; }
    }
}
