using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesInRenewalQueryBase : BaseRequestType
    {

       
        public bool PartyKeySpecified { get; set; }
       
        public string ProductCode { get; set; }
        public string InsuranceRef { get; set; }
        public int PartyKey { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public bool RenewalDateSpecified { get; set; }
        public bool SearchTypeSpecified { get; set; }
        public bool RetrieveAssociates { get; set; }
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
        public bool DirectOnly { get; set; }
        public bool DirectOnlySpecified { get; set; }
        public bool ForAccept { get; set; }
        public bool ForAcceptSpecified { get; set; }
        public ContactUserSearchType SearchType { get; set; }
        

    }
}
