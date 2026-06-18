using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetQuotesMarkedForCollectionRequestType : BaseRequestType
    {
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
        public bool DirectBusinessOnly { get; set; }
        public bool DirectBusinessOnlySpecified { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool InsuranceFileKeySpecified { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
        public System.Collections.Generic.List<BaseGetQuotesMarkedForCollectionRequestTypeProducts> Products { get; set; }
        public System.DateTime SearchDateFrom { get; set; }
        public bool SearchDateFromSpecified { get; set; }
        public System.DateTime SearchDateTo { get; set; }
        public bool SearchDateToSpecified { get; set; }
    }
}
