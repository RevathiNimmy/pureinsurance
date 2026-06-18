using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPoliciesInRenewalRequestType : BaseRequestType
    {
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
        public bool DirectOnly { get; set; }
        public bool DirectOnlySpecified { get; set; }
        public bool ForAccept { get; set; }
        public bool ForAcceptSpecified { get; set; }
        public string InsuranceRef { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        public string ProductCode { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public bool RenewalDateSpecified { get; set; }
        public bool RetrieveAssociates { get; set; }
        public ContactUserSearchType SearchType { get; set; }
        public bool SearchTypeSpecified { get; set; }
    }
}
