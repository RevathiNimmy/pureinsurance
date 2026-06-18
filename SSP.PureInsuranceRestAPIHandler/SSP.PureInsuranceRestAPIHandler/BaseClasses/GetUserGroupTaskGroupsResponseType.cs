using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupTaskGroupsResponseType : BaseResponseType
    {
        public List<BaseGetUserGroupTaskGroupsResponseTypeRow> TaskGroups { get; set; }

        public byte[] TimeStamp { get; set; }
    }
}
