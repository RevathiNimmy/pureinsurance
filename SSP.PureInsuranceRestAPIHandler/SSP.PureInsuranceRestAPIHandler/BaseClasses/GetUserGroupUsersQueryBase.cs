
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupUsersQueryBase :BaseRequestType
    {
        public DateTime EffectiveDate { get; set; }
        public bool RestrictUserList { get; set; }
        public bool RestrictUserListSpecified { get; set; }
        public string UserGroupCode { get; set; }
        public int AgentKey { get; set; }
    }
}
