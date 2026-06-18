using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTaxesAndFeesType
    {

        public decimal PolicyFees { get; set; }

        public decimal PolicyTaxes { get; set; }

        public decimal RiskFees { get; set; }

        public decimal RiskTaxes { get; set; }
        public System.Collections.Generic.List<BaseFeesType> Fees { get; set; }
        public System.Collections.Generic.List<BaseTaxesType> Taxes { get; set; }
    }
}
