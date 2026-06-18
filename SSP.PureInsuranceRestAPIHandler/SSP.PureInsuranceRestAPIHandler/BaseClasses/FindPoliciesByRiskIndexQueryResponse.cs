using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPoliciesByRiskIndexQueryResponse : BaseResponseType
    {
        public string InsuranceFolderKeys { get; set; }
        public string InsuranceFileKeys { get; set; }
        public string RiskKeys { get; set; }
    }
}