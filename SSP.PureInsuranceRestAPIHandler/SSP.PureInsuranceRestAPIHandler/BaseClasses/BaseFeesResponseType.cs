namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFeesResponseType
    {
        public string FeeName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public string AppliedTo { get; set; } = string.Empty;
        public double Premium { get; set; } = 0;
        public double Rate { get; set; } = 0;
        public double FeeAmount { get; set; } = 0;
        public double TaxAmount { get; set; } = 0;
        public double TotalAmount { get; set; } = 0;
        public string TaxGroup { get; set; } = string.Empty;
        public int IncludeInInstallment { get; set; } = 0;
        public int SpreadAcrossInstallment { get; set; } = 0;
        public bool IsValue { get; set; } = false;
        public int PolicyFeeKey { get; set; } = 0;
    }
}
