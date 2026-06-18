using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserAuthorityValueRequest : BaseRequestType
    {
        public UserAuthorityOptions UserAuthorityOption { get; set; }
        public string UserCode { get; set; }
    }
}
