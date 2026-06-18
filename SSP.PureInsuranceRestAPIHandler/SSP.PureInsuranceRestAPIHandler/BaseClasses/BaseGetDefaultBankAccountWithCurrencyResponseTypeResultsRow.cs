namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow
    {
        //[DBCol("BankAccountCode")]
        public string BankAccountCode { get; set; } = string.Empty;
        //[DBCol("BankAccountDefaultID")]
        public int BankAccountDefaultID { get; set; }
        //[DBCol("BankAccountID")]
        public int BankAccountID { get; set; }
        //[DBCol("CashListTypeCode")]
        public string CashListTypeCode { get; set; } = string.Empty;
        //[DBCol("CashListTypeID")]
        public int CashListTypeID { get; set; }
        //[DBCol("Code")]
        public string Code { get; set; } = string.Empty;
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; } = string.Empty;
        //[DBCol("CurrencyID")]
        public int CurrencyID { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; } = string.Empty;
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("MediaTypeCode")]
        public string MediaTypeCode { get; set; } = string.Empty;
        //[DBCol("MediaTypeID")]
        public int MediaTypeID { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; } = string.Empty;
        //[DBCol("ProductID")]
        public int ProductID { get; set; }
        //[DBCol("SourceID")]
        public int SourceID { get; set; }
    }
}
