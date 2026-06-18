using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePremiumFinancePlanInstalmentsType
    {
        //[DBCol("Amount")]
        public double Amount { get; set; }
        //[DBCol("Batch_Ref")]
        public string BatchRef { get; set; }
        //[DBCol("Commission")]
        public double Commission { get; set; }
        //[DBCol("currency_desc")]
        public string CurrencyDesc { get; set; }
        //[DBCol("DueDate")]
        public System.DateTime DueDate { get; set; }
        //[DBCol("Export_Date")]
        public System.DateTime ExportDate { get; set; }
        //[DBCol("Fee")]
        public double Fee { get; set; }
        public System.Collections.Generic.List<BasePremiumFinancePlanInstalmentsHistoryType> History { get; set; }
        //[DBCol("InstalmentNumber")]
        public int InstalmentNumber { get; set; }
        //[DBCol("reason_description")]
        public string InstalmentReason { get; set; }
        //[DBCol("pfinstalments_resultCode")]
        public string InstalmentReasonCode { get; set; }
        //[DBCol("PfInstalments_id")]
        public int PFInstalmentsKey { get; set; }
        //[DBCol("PFTransaction_id")]
        public int PFTransactionKey { get; set; }
        //[DBCol("PostedDate")]
        public System.DateTime PostedDate { get; set; }
        //[DBCol("status_Code")]
        public string StatusCode { get; set; }
        //[DBCol("status_description")]
        public string StatusDescription { get; set; }
        //[DBCol("Tax")]
        public double Tax { get; set; }
        //[DBCol("transaction_description")]
        public string TransactionDescription { get; set; }
    }
}
