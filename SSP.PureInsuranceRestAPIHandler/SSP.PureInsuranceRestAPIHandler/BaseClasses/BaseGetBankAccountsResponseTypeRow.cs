namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetBankAccountsResponseTypeRow
    {
        //[DBCol("BankAccountKey")]
        public int BankAccountKey { get; set; }
        //[DBCol("BankAccountName")]
        public string BankAccountName { get; set; }
        //[DBCol("BankAccountNumber")]
        public string BankAccountNumber { get; set; }
        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CurrencyKey")]
        public int CurrencyKey { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("IsDeleted")]
        public int IsDeleted { get; set; }
    }
}
