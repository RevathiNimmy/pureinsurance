namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetCurrencyExchangeRates
{
    public class GetCurrencyExchangeRatesQueryBase : BaseRequestType
    {
        public string AccountCode { get; set; }
        public int ClaimKey { get; set; }
        public decimal CurrencyAmountUnRounded { get; set; }
        public bool CurrencyAmountUnRoundedSpecified { get; set; }
        public bool IsManualJournal { get; set; }
        public string Mode { get; set; }
        
        public string TransactionCurrencyCode { get; set; }
    }
}
