using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupUsersRequestType : BaseRequestType
    {
        public string  EffectiveDate { get; set; }

        public Boolean RestrictUserList { get; set; }

        public Boolean RestrictUserListSpecified { get; set; }

        public string UserGroupCode { get; set; }
    }
}
