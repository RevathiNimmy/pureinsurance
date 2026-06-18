namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseCalculateTaxForClaimsRequestType : BaseRequestType
    {
        public string CompanyCode { get; set; }
        public string TaxGroupCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LossCurrencyCode { get; set; }
        public double Amount { get; set; }
        public int CompanyKey { get; set; }
        public int TaxGroupKey { get; set; }
        public int CurrencyKey { get; set; }
        public int LossCurrencyKey { get; set; }
        public int PerilId { get; set; }
        public string TransactionTypeCode { get; set; }
        public string ReserveType { get; set; }
        public int ReserveKey { get; set; }
        public bool IsSalvageRecovery { get; set; }
        public BaseClaimReceiptAdvancedTaxDetailsType ReceiptAdvancedTaxDetails { get; set; }
        public BaseClaimPaymentAdvancedTaxDetailsType AdvancedTaxDetails { get; set; }
        public string PaymentTo { get; set; }
    }
}
