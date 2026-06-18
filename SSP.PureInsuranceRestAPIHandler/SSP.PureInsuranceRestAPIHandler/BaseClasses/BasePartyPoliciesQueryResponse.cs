using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyPoliciesQueryResponse
    {
        public string PartyCode { get; set; }
        public int PartyKey { get; set; }
        public string PartyName { get; set; }
        public System.Collections.Generic.List<BaseGetPartyPoliciesResponseTypeRow> PartyPolicies { get; set; }
        public int SourceKey { get; set; }
    }
}
