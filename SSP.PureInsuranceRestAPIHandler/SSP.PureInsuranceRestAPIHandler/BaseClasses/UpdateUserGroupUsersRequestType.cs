using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateUserGroupUsersRequestType : BaseRequestType
    {
        public byte[] TimeStamp { get; set; }

        public int UserGroupKey { get; set; }

        public List<BaseUpdateUserGroupUsersRequestTypeUsersRow> Users;
    }
}
