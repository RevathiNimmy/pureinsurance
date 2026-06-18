namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentItemDetailsType
    {
        public int BaseClaimPaymentItemKey { get; set; }
        public int BaseReserveKey { get; set; }
        public int BaseRecoveryKey { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PaymentAdjustment { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public int ReserveKey { get; set; }
        public string TaxGroupCode { get; set; }
        public decimal ThisRevision { get; set; }
        public decimal LossAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal LossTaxAmount { get; set; }
        public string BaseCurrencyCode { get; set; }
    }

}