using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateInsuranceFileCommandBase : BaseRequestType
    {
        public int InsuranceFileCnt { get; set; }
        public double? CurrencyBaseRate { get; set; }
        public DateTime? CurrencyBaseDate { get; set; }
        public double? AccountBaseRate { get; set; }
        public DateTime? AccountBaseDate { get; set; }
        public double? SystemBaseRate { get; set; }
        public DateTime? SystemBaseDate { get; set; }
        public int? RateOverrideReasonID { get; set; }
        public int? BaseCurrencyID { get; set; }
        public int? AccountCurrencyID { get; set; }
    }
}
