namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseInsurerType
    {
        public int RecoveryId { get; set; }
        public decimal ThisRecoveryAmount { get; set; }
        public decimal ThisRecoveryTaxAmount { get; set; }
        public string RecoveryTypeDescription { get; set; }
        public int PartyCnt { get; set; }
        public string PartyName { get; set; }
        public decimal SharePercentage { get; set; }
        public decimal RecoveryToDateLC { get; set; }
        public int IsTaxShared { get; set; }
        public int RIArrangementLineId { get; set; }
        public int TreatyId { get; set; }
        public decimal ThisRecoverySplitAmount { get; set; }
        public decimal ThisRecoverySplitAmountLC { get; set; }
        public decimal ThisRecoveryTaxAmountLC { get; set; }
        public decimal ThisRecoveryAmountLC { get; set; }
        public decimal ReceiptToLossXRate { get; set; }
        public decimal ThisRecoverySplitTaxAmountLC { get; set; }
        public decimal ThisRecoverySplitTaxAmount { get; set; }
    }
}
