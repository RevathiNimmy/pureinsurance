namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCurrencyToCurrencyExchangeRateQueryBase : BaseRequestType
    {
        public decimal CurrencyAmountUnRounded { get; set; }
        public bool CurrencyAmountUnRoundedSpecified { get; set; }
        public string CurrencyCodeFrom { get; set; }
        public string CurrencyCodeTo { get; set; }
    }
}
