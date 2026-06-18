namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPerilSummaryResponseTypeRow1
    {
        //[DBCol("CurrentRecovery")]
        public decimal CurrentRecovery { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("InitialRecovery")]
        public decimal InitialRecovery { get; set; }
        //[DBCol("ReceiptedAmount")]
        public decimal ReceiptedAmount { get; set; }
        //[DBCol("RevisedRecovery")]
        public decimal RevisedRecovery { get; set; }
    }
}
