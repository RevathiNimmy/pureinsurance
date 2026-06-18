using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PolicyProcessV2CommandBase : BaseRequestType
    {
        public string AgentCode { get; set; }
        public BasePartyCCType CorporateClient { get; set; }
        public CurrencyType CurrencyCode { get; set; }
        public bool CurrencyCodeSpecified { get; set; }
        public BasePartyPCType PersonalClient { get; set; }
        public BaseQuoteRiskMsgType Policy { get; set; }
        public bool UpdateParty { get; set; }
    }
}
