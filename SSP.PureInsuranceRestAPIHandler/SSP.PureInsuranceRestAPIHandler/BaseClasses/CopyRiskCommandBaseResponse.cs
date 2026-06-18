namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CopyRiskCommandBaseResponse : BaseResponseType
    {
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public byte[] QuoteTimeStamp { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        public int RiskKey { get; set; }
    }
}
