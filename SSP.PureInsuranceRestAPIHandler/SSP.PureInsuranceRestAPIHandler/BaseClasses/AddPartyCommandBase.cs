namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddPartyCommandBase : BaseRequestType
    {
        public BasePartyPCType BasePartyPCType { get; set; } = null;
        public BasePartyCCType BasePartyCCType { get; set; } = null;
        public BasePartyOtherType BasePartyOTHERType { get; set; } = null;
        public string SubBranchCode { get; set; } = string.Empty;
        public int AgentKey { get; set; }
    }
}
