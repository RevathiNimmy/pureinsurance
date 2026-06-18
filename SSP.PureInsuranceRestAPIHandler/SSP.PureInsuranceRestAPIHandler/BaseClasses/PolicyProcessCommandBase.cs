using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PolicyProcessCommandBase : BaseRequestType
    {

        public string AgentCode { get; set; }
        public bool ClientCodeSpecified { get; set; }
        public int ClientID { get; set; }
        public CurrencyType CurrencyCode { get; set; }
        public bool CurrencyCodeSpecified { get; set; }
        public BasePartyType Item { get; set; }
        public BaseQuoteRiskMsgType Policy { get; set; }
        public bool UpdateParty { get; set; }
    }
}
