namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalSelectionByPolicyCommandBaseResponse : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public int RenewalInsuranceFileKey { get; set; }

        public string RenewalInsuranceQuoteRef { get; set; }
    }
}
