namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRecommendStatusCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public short Status { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
