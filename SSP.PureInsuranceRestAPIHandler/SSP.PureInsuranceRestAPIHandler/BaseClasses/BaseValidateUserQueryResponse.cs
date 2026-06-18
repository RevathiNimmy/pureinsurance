using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseValidateUserQueryResponse : BaseResponseType
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public System.Collections.Generic.List<string> PasswordHistory { get; set; } = new List<string>();
        public bool SystemUpgradeChangePasswordRequired { get; set; }
    }
}
