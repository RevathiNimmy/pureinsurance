namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetCurrencyExchangeRates
{
    public class GetCurrencyExchangeRatesQueryResponse
    {
        public decimal AccountAmount { get; set; }
        public decimal AccountAmountUnrounded { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal BaseAmountUnrounded { get; set; }
        public BaseCoreCurrencyExchangeRatesType CurrencyRates { get; set; }
        public decimal SystemAmount { get; set; }
        public decimal SystemAmountUnrounded { get; set; }
    }
}
