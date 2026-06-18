namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetListOfManualJournalTransactionDetailsResponse
    {
        //[DBCol("AccountCode")]
        public string AccountCode { get; set; }
        //[DBCol("AlternateRef")]
        public string AlternateRef { get; set; }
        //[DBCol("Amount")]
        public decimal Amount { get; set; }
        //[DBCol("BaseAmount")]
        public decimal BaseAmount { get; set; }
        //[DBCol("Comment")]
        public string Comment { get; set; }
        //[DBCol("CostCentreDescription")]
        public string CostCentreDescription { get; set; }
        //[DBCol("CurrencyRate")]
        public decimal CurrencyRate { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("TransDetailId")]
        public int TransDetailsId { get; set; }
        //[DBCol("PurchaseInvoiceNumber")]
        public string PurchaseInvoiceNumber { get; set; }
        //[DBCol("PurchaseOrderNumber")]
        public string PurchaseOrderNumber { get; set; }
        //[DBCol("UnderwritingYearDescription")]
        public string UnderwritingYearDescription { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CurrencyTypeDescription")]
        public string CurrencyTypeDescription { get; set; }
        //[DBCol("ManualJournalDetailId")]
        public int ManualJournalDetailId { get; set; }
    }
}
