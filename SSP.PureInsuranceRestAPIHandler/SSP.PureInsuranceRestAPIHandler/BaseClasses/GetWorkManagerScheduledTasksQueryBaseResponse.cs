using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWorkManagerScheduledTasksQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetWorkManagerScheduledTasksResponseTypeRow> Tasks { get; set; }
    }
}
