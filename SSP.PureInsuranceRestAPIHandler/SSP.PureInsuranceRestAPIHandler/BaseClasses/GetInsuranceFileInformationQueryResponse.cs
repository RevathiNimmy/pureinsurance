using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInsuranceFileInformationQueryResponse : BaseResponseType
    {
        public int CompanyId { get; set; }
        public int AccountId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Premium { get; set; }
        public double CurrencyBaseXrate { get; set; }
        public DateTime CurrencyBaseDate { get; set; }
        public double AccountBaseXrate { get; set; }
        public DateTime AccountBaseDate { get; set; }
        public double SystemBaseXrate { get; set; }
        public DateTime SystemBaseDate { get; set; }
        public int RateOverrideReasonId { get; set; }
    }
}
