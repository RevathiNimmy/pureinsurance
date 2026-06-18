
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupUsersQueryBaseResponse :BasePagedResponse
    {
        public string EmailAddress { get; set; }
        public List<BaseGetUserGroupUsersResponseTypeRow> UserGroupUsers { get; set; }
    }
}
