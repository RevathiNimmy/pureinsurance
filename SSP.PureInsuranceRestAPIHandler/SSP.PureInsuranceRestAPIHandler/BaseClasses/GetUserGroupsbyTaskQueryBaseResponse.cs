
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsbyTaskQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetUserGroupsbyTaskResponseTypeRow> UserGroups { get; set; }
    }
}
