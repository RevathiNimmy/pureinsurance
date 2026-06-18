namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCurrencyToCurrencyExchangeRateQueryResponse
    {
        public decimal BaseAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal CurrencyAmount { get; set; }
    }
}
