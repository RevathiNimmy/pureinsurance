namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetReferredPaymentsResponseTypeRow
    {
        public string CaseNumber { get; set; }
        public int ClaimKey { get; set; }
        public string ClaimNumber { get; set; }
        public int ClaimPaymentKey { get; set; }
        public string ClientName { get; set; }
        public string CreatedBy { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyId { get; set; }
        public bool IsReferredForRecommendation { get; set; }
        public string PayeeName { get; set; }
        public string PayeeType { get; set; }
        public double PaymentAmount { get; set; }
        public double PaymentAmountBaseCurrency { get; set; }
        public bool PaymentAmountBaseCurrencySpecified { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public string PolicyNumber { get; set; }
        public string RecommendedBy { get; set; }
        public string Status { get; set; }
    }
}
