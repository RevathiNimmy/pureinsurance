using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserAuthorityValueQueryBase : BaseRequestType
    {
        public UserAuthorityOptions UserAuthorityOption { get; set; }
        public string UserCode { get; set; }
        public int AgentKey { get; set; }
    }
}
