namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteRiskCommandBaseResponse : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
