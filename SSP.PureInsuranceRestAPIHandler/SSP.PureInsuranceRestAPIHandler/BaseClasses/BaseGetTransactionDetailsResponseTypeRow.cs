namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTransactionDetailsResponseTypeRow
    {
        public string AccountCode { get; set; }
        public int Accountkey { get; set; }
        public byte[] AllocationTimeStamp { get; set; }
        public string AltRef { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyDiff { get; set; }
        public string DocRef { get; set; }
        public string DocType { get; set; }
        public int DocTypeID { get; set; }
        public string DoctypeGroup { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int InsuranceFileCnt { get; set; }
        public string InsuranceRef { get; set; }
        public string MediaRef { get; set; }
        public string MediaType { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string PeriodName { get; set; }
        public string PrimarySettled { get; set; }
        public int SourceID { get; set; }
        public string Spare { get; set; }
        public string TaxBand { get; set; }
        public System.DateTime TransDate { get; set; }
        public int TransDetailKey { get; set; }
        public decimal TransactionCurrenciesAmount { get; set; }
        public string TransactionCurrency { get; set; }
        public string TransactionCurrencyCode { get; set; }
        public System.DateTime DueDate { get; set; }
    }
}
