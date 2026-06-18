using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CancelPremiumFinancePlanCommandBaseResponse : BaseResponseType
    {
        public int DebitTransdetailKey { get; set; }
        public List<BaseCancelPremiumFinancePlanResponseTypePolicies> PFPolicies { get; set; }
        public string Warnings { get; set; }
    }
}
