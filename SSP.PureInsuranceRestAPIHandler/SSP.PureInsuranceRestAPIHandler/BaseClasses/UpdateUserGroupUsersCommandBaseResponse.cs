
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateUserGroupUsersCommandBaseResponse : BaseResponseType
    {
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
