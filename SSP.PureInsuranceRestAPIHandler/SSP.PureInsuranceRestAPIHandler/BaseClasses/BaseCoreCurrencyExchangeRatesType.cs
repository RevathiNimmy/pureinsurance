namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCoreCurrencyExchangeRatesType
    {
        public System.DateTime AccountCurrencyDate { get; set; }
        public string AccountCurrencyDesc { get; set; }
        public int AccountCurrencyKey { get; set; }
        public decimal AccountCurrencyRate { get; set; }
        public bool AccountCurrencyRateSpecified { get; set; }
        public System.DateTime BaseCurrencyDate { get; set; }
        public string BaseCurrencyDesc { get; set; }
        public int BaseCurrencyKey { get; set; }
        public decimal BaseCurrencyRate { get; set; }
        public bool BaseCurrencyRateSpecified { get; set; }
        public int ExchangeRateOverrideReasonKey { get; set; }
        public bool ExchangeRateOverrideReasonKeySpecified { get; set; }
        public System.DateTime SystemCurrencyDate { get; set; }
        public int SystemCurrencyKey { get; set; }
        public decimal SystemCurrrencyRate { get; set; }
        public bool SystemCurrrencyRateSpecified { get; set; }
        public string TransactionCurrencyDesc { get; set; }
        public int TransactionCurrencyKey { get; set; }
        public decimal TransactionCurrrencyRate { get; set; }
        public bool TransactionCurrrencyRateSpecified { get; set; }
    }
}
