using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePremiumFinancePlanTransactionsType
    {
        //[DBCol("short_code")]
        public string AccountCode { get; set; }
        //[DBCol("account_id")]
        public int Accountkey { get; set; }
        //[DBCol("alternate_reference")]
        public string AltRef { get; set; }
        //[DBCol("Amount")]
        public double Amount { get; set; }
        //[DBCol("currency_description")]
        public string Currency { get; set; }
        public decimal CurrencyDiff { get; set; } = 0;

        //[DBCol("document_ref")]
        public string DocRef { get; set; }
        //[DBCol("DocumentType_Descritpion")]
        public string DocType { get; set; }
        //[DBCol("documenttype_id")]
        public int DocTypeID { get; set; }
        //[DBCol("DocumentTypeGroup_Description")]
        public string DoctypeGroup { get; set; }
        //[DBCol("cover_start_date")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("insurance_file_cnt")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("insurance_ref")]
        public string InsuranceRef { get; set; }
        //[DBCol("insurance_ref_index")]
        public int InsuranceRefIndex { get; set; }
        //[DBCol("spare")]
        public string MediaRef { get; set; }
        //[DBCol("media_type")]
        public string MediaType { get; set; }
        //[DBCol("outstanding_amount")]
        public decimal OutstandingAmount { get; set; }
        //[DBCol("pftransaction_id")]
        public int PFTransactionKey { get; set; }
        //[DBCol("period_name")]
        public string PeriodName { get; set; }
        public string PrimarySettled { get; set; }
        //[DBCol("company_id")]
        public int SourceID { get; set; }
        //[DBCol("spare")]
        public string Spare { get; set; }
        //[DBCol("code")]
        public string TaxBand { get; set; }
        //[DBCol("document_date")]
        public System.DateTime TransDate { get; set; }
        //[DBCol("transdetail_id")]
        public int TransDetailKey { get; set; }
        //[DBCol("currency_amount")]
        public decimal TransactionCurrenciesAmount { get; set; }
        //[DBCol("currency_description")]
        public string TransactionCurrency { get; set; }
        //[DBCol("currency_Code")]
        public string TransactionCurrencyCode { get; set; }

    }
}
