namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdatePolicyPaymentMethodCommandResponse : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; }
        public STSErrorType ErrorType { get; set; }
    }
}
