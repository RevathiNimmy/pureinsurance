using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DoCurrencyConversionQueryBase : BaseRequestType
    {
        public int AccountId { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyAmountUnrounded { get; set; }
        public int BaseCurrencyId { get; set; }
        public decimal BaseAmount { get; set; }
        public int AccountCurrencyId { get; set; }
        public decimal AccountAmount { get; set; }
        public int SystemCurrencyId { get; set; }
        public decimal SystemAmount { get; set; }
        public decimal CurrencyBaseXrate { get; set; }
        public DateTime CurrencyBaseDate { get; set; }
        public decimal AccountBaseXrate { get; set; }
        public DateTime AccountBaseDate { get; set; }
        public decimal SystemBaseXrate { get; set; }
        public DateTime SystemBaseDate { get; set; }
    }
}
