using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsQueryResponse : BasePagedResponse
    {
        public List<BaseGetUserGroupsResponseTypeRow> UserGroups { get; set; }

    }
}
