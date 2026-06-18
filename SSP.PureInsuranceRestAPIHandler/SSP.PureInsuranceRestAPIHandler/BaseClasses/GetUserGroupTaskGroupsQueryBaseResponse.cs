
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupTaskGroupsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetUserGroupTaskGroupsResponseTypeRow> TaskGroups { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
