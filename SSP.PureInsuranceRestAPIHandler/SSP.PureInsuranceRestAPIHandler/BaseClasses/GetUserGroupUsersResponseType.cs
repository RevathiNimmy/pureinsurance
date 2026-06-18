using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupUsersResponseType : BaseResponseType
    {
        public string EmailAddress { get; set; }

        public List<BaseGetUserGroupUsersResponseTypeRow> UserGroupUsers { get; set; }
    }
}
