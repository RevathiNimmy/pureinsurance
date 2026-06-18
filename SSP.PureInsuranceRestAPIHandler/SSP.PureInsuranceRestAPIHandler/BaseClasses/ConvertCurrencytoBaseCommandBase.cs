namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ConvertCurrencytoBaseCommandBase : BaseRequestType
    {
        public int CurrencyID { get; set; }
        public int CompanyID { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public object? ConversionDate { get; set; }
        public object? ConversionRate { get; set; }
        public bool IsMultiplier { get; set; }
        public object? Rounded { get; set; }
        public object? BaseRoundingDifference { get; set; }
        public object? CurrencyRoundingDifference { get; set; }
        public object? FormattedBase { get; set; }
        public object? FormattedCurrency { get; set; }
        public int Euro { get; set; }
        public decimal EuroAmount { get; set; }
        public object? EuroCCyXrate { get; set; }
        public object? EuroBaseXRate { get; set; }
        public object? CcyAmountUnRounded { get; set; }
        public decimal BaseAmountUnRounded { get; set; }
    }
}
