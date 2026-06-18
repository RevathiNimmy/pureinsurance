using SSP.PureInsuranceRestAPIHandler.BaseClasses;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindPolicyTransactionGrouped
{
    public class BasePolicyTransactionGroupedRequest : BaseRequestType
    {
        public string AgentCode { get; set; }
        public bool AgentCodeSpecified { get; set; }
        public string ClientCode { get; set; }
        public bool ClientCodeSpecified { get; set; }
        public System.DateTime DueDate { get; set; }
        public bool DueDateSpecified { get; set; }
        public System.DateTime EffectiveFromDate { get; set; }
        public bool EffectiveFromDateSpecified { get; set; }
        public System.DateTime EffectiveToDate { get; set; }
        public bool EffectiveToDateSpecified { get; set; }
        public bool OnlyOutstanding { get; set; }
        public string PolicyReference { get; set; }
        public bool PolicyReferenceSpecified { get; set; }
    }
}
