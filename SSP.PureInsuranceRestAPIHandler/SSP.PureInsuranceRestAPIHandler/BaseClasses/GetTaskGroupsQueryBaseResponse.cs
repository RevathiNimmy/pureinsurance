using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskGroupsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetTaskGroupsResponseTypeRow> TaskGroups { get; set; }
    }
}
