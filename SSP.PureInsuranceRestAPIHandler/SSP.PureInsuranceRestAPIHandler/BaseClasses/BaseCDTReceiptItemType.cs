namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtReceiptItemType
    {
        public decimal ReceiptAmount { get; set; }
        public string RecoveryTypeCode { get; set; }
        public int SAMStagingClaimReceiptItemKey { get; set; }
        public string TaxGroupCode { get; set; }
        public long SiriusBaseRecoveryKey { get; set; }
    }
}
