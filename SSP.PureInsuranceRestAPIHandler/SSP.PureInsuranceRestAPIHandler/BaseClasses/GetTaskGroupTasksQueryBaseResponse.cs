using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskGroupTasksQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetTaskGroupTasksResponseTypeRow> TaskGroupTasks { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
