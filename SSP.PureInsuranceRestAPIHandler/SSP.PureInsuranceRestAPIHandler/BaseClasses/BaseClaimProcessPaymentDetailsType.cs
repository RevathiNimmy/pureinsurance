using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessPaymentDetailsType
    {
        public int PaymentPartyKey { get; set; }
        public string PaymentMediaTypeCode { get; set; }
        public string PaymentMediaReference { get; set; }
        public string PaymentPayee { get; set; }
        public string PaymentBankCode { get; set; }
        public string PaymentCurrencyCode { get; set; }
        public ClaimPaymentPartyTypeType PaymentPartyType { get; set; }
        public string UltimatePayee { get; set; }
    }
}
