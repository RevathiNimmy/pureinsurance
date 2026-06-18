using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePolicyProcessV2ResponseTypeRisks : BaseRiskResponseType
    {
        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypeRiskTaxes> Taxes { get; set; }
        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypeRiskFees> Fees { get; set; }
        public System.Collections.Generic.List<BasePolicyProcessV2ResponseTypeRatingSections> RatingSections { get; set; }
        public string RiskStatus { get; set; }
        public int RiskFolderKey { get; set; }
    }
}
