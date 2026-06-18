namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPerilRecoveryReceiptType
    {
        public int BaseRecoveryKey { get; set; }
        public string TypeCode { get; set; }
        public decimal TotalRecoveryAmount { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal ThisReceiptINCLTaxAmount { get; set; }
        public decimal ThisReceiptTaxAmount { get; set; }
        public decimal ThisReceiptNetAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public int RecoveryId { get; set; }
        public int RecoveryTypeId { get; set; }
    }
}
