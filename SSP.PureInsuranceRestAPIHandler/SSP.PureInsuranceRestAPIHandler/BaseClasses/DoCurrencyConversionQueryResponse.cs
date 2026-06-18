using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DoCurrencyConversionQueryResponse : BaseResponseType
    {
        public int BaseCurrencyId { get; set; }
        public decimal BaseAmount { get; set; }
        public int AccountCurrencyId { get; set; }
        public decimal AccountAmount { get; set; }
        public int SystemCurrencyId { get; set; }
        public decimal SystemAmount { get; set; }
        public double CurrencyBaseXrate { get; set; }
        public DateTime CurrencyBaseDate { get; set; }
        public double AccountBaseXrate { get; set; }
        public DateTime AccountBaseDate { get; set; }
        public double SystemBaseXrate { get; set; }
        public DateTime SystemBaseDate { get; set; }
    }
}
