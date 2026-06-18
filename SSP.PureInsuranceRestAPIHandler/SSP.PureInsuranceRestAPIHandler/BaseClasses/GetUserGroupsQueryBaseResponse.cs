using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetUserGroupsResponseTypeRow> UserGroups { get; set; }
    }
}
