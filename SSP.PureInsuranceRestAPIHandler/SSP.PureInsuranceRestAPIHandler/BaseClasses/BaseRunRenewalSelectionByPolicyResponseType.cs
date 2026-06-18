namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRunRenewalSelectionByPolicyResponseType : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; }
        public int RenewalInsuranceFileKey { get; set; }
        public string RenewalInsuranceQuoteRef { get; set; }
    }
}
