using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ValidateUserQueryResponse : BaseResponseType
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public List<string> PasswordHistory { get; set; } = new List<string>();
        public bool SystemUpgradeChangePasswordRequired { get; set; }
    }
}
