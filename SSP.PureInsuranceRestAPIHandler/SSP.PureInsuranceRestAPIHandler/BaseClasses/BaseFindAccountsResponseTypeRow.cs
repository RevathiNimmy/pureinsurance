namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindAccountsResponseTypeRow
    {
        //[DBCol("AccountBalance")]
        public double AccountBalance { get; set; }
        //[DBCol("AccountKey")]
        public int AccountKey { get; set; }
        //[DBCol("AccountName")]
        public string AccountName { get; set; }
        //[DBCol("AccountStatus")]
        public string AccountStatus { get; set; }
        //[DBCol("AccountStatusKey")]
        public int AccountStatusKey { get; set; }
        //[DBCol("AccountTypeCode")]
        public string AccountTypeCode { get; set; }
        //[DBCol("AccountTypeKey")]
        public int AccountTypeKey { get; set; }
        //[DBCol("AddressLine1")]
        public string AddressLine1 { get; set; }
        //[DBCol("CompanyKey")]
        public int CompanyKey { get; set; }
        //[DBCol("ContactName")]
        public string ContactName { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("CurrencyId")]
        public int CurrencyId { get; set; }
        //[DBCol("FullKey")]
        public string FullKey { get; set; }
        //[DBCol("IsGrossAgent")]
        public string IsGrossAgent { get; set; }
        //[DBCol("LedgerCode")]
        public string LedgerCode { get; set; }
        //[DBCol("LedgerKey")]
        public int LedgerKey { get; set; }
        //[DBCol("NominalAccountKey")]
        public int NominalAccountKey { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PersonalClientForename")]
        public string PersonalClientForename { get; set; }
        //[DBCol("ShortCode")]
        public string ShortCode { get; set; }
        //[DBCol("SourceCode")]
        public string SourceCode { get; set; }
        //[DBCol("SourceId")]
        public int SourceId { get; set; }
    }
}
