using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePolicyProcessV2ResponseTypePolicy : BasePolicyProcessV2ResponseTypePolicyDetails
    {
        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypeRisks> Risks { get; set; }

        public BasePolicyProcessV2ResponseTypeCommission Commission { get; set; }

        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypePolicyTaxes> Taxes { get; set; }

        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypePolicyFees> Fees { get; set; }

    }
}
