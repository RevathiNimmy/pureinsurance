namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentSettingsQueryBase : BaseRequestType
    {
        public int AgentKey { get; set; }
        public bool AgentKeySpecified { get; set; }
    }
}
