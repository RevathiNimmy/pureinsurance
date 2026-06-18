using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWorkManagerScheduledTasksQueryResponse : BasePagedResponse
    {
        public List<BaseGetWorkManagerScheduledTasksResponseTypeRow> Tasks { get; set; }
    }
}
