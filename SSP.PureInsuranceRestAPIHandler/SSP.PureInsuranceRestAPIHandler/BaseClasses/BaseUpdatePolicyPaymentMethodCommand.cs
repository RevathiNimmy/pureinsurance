namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdatePolicyPaymentMethodCommand : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public string PolicyPaymentMethod { get; set; }
        public byte[] QuoteTimeStamp { get; set; }
    }
}
