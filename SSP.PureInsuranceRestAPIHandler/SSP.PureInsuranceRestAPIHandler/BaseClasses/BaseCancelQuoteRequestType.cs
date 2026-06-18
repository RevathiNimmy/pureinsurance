namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCancelQuoteRequestType : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = null;
    }
}
