using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsbyTaskResponseType : BaseResponseType
    {
        public List<BaseGetUserGroupsbyTaskResponseTypeRow> UserGroups { get; set; }
    }
}
