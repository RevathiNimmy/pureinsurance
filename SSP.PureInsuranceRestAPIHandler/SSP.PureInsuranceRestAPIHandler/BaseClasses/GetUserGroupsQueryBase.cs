namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsQueryBase : BaseRequestType
    {
        public bool ShowForCurrentUserOnly { get; set; }
        public int AgentKey { get; set; }
    }
}
