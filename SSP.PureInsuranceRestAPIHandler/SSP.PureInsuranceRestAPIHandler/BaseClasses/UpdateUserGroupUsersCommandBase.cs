
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateUserGroupUsersCommandBase :BaseRequestType
    {
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public int UserGroupKey { get; set; }
        public List<BaseUpdateUserGroupUsersRequestTypeUsersRow> Users { get; set; }
    }
}
