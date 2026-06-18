namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReceiptItemDetailsType
    {
        public int BaseClaimReceiptItemKey { get; set; }
        public int BaseRecoveryKey { get; set; }
        public int BaseReserveKey { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ReceiptBaseAmount { get; set; }
        public decimal ReceiptLossAmount { get; set; }
        public int ClaimReceiptItemKey { get; set; }
        public string RecoveryTypeCode { get; set; }
        public string TaxGroupCode { get; set; }
    }

}