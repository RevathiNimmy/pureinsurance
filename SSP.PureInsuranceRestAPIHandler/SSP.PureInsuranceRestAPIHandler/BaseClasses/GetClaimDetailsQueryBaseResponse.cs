namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimDetailsQueryBaseResponse : BaseResponseType
    {
        public BaseGetClaimDetailsType ClaimDetails { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
