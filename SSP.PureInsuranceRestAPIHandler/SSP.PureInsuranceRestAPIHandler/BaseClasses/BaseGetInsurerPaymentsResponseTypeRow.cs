namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetInsurerPaymentsResponseTypeRow
    {
        //[DBCol("account_amount")]
        public decimal AccountAmount { get; set; }
        //[DBCol("account_base_xrate")]
        public decimal AccountBaseRate { get; set; }
        //[DBCol("code1")]
        public string AccountCurrencyCode { get; set; }
        //[DBCol("account_currency_id")]
        public int AccountCurrencyId { get; set; }
        //[DBCol("accounting_date")]
        public System.DateTime AccountingDate { get; set; }
        //[DBCol("alternate_reference")]
        public string AlternateReference { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("client_outstanding")]
        public decimal ClientOutstanding { get; set; }
        //[DBCol("client_outstanding_account_amount")]
        public decimal ClientOutstandingAccountAmount { get; set; }
        //[DBCol("company_id")]
        public int CompanyId { get; set; }
        //[DBCol("consolidate_binder")]
        public int ConsolidateBinder { get; set; }
        //[DBCol("currency_amount")]
        public decimal CurrencyAmount { get; set; }
        //[DBCol("currency_base_xrate")]
        public decimal CurrencyBaseRate { get; set; }
        //[DBCol("code")]
        public string CurrencyCode { get; set; }
        //[DBCol("currency_id")]
        public int CurrencyId { get; set; }
        //[DBCol("document_id")]
        public int DocumentId { get; set; }
        //[DBCol("document_ref")]
        public string DocumentRef { get; set; }
        //[DBCol("due_date")]
        public System.DateTime DueDate { get; set; }
        //[DBCol("effective_date")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("fully_paid_account_amount")]
        public decimal FullyPaidAccountAmount { get; set; }
        //[DBCol("fully_paid_amount")]
        public decimal FullyPaidAmount { get; set; }
        //[DBCol("insurer_ref")]
        public string InsurerRef { get; set; }
        //[DBCol("marked_account_amount")]
        public decimal MarkedAccountAmount { get; set; }
        //[DBCol("marked_amount")]
        public decimal MarkedAmount { get; set; }
        //[DBCol("month")]
        public int Month { get; set; }
        //[DBCol("paid_account_amount")]
        public decimal PaidAccountAmount { get; set; }
        //[DBCol("paid_amount")]
        public decimal PaidAmount { get; set; }
        //[DBCol("period_name")]
        public string PeriodName { get; set; }
        //[DBCol("resolved_name")]
        public string ResolvedName { get; set; }
        //[DBCol("shortname")]
        public string ShortName { get; set; }
        //[DBCol("spare")]
        public string Spare { get; set; }
        //[DBCol("transdetail_id")]
        public int TransdetailId { get; set; }
        //[DBCol("year_name")]
        public string YearName { get; set; }
    }
}
