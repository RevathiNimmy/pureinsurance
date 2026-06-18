namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetListOfManualJournalTransactionsResponse
    {
        //[DBCol("AccountCode")]
        public string AccountCode { get; set; }
        //[DBCol("Alternate_Ref")]
        public string AlternateRef { get; set; }
        //[DBCol("Amount")]
        public decimal Amount { get; set; }
        //[DBCol("Base_Amount")]
        public decimal BaseAmountRate { get; set; }
        //[DBCol("Comment")]
        public string Comment { get; set; }
        //[DBCol("CostCenterId")]
        public string CostCenterId { get; set; }
        //[DBCol("Currency_Id")]
        public int Currency { get; set; }
        //[DBCol("Currency_Rate")]
        public decimal CurrencyRate { get; set; }
        //[DBCol("Insurance_Ref")]
        public string InsuranceRef { get; set; }
        //[DBCol("ManualJournal_Id")]
        public int ManualJournalId { get; set; }
        //[DBCol("Purchase_Invoice_No")]
        public string PurchaseInvoiceNumber { get; set; }
        //[DBCol("Purchase_Order_No")]
        public string PurchaseOrderNumber { get; set; }
        //[DBCol("UnderwritingYear_Id")]
        public string UnderwritingYearId { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CreatedBy")]
        public string CreatedBy { get; set; }
        //[DBCol("CreatedDate")]
        public System.DateTime CreatedDate { get; set; }
        //[DBCol("Status")]
        public string Status { get; set; }
    }
}
