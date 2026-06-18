using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateTaskGroupTasksCommandBase : BaseRequestType
    {
        public int TaskGroupKey { get; set; }
        public List<BaseUpdateTaskGroupTasksRequestTypeRow> Tasks { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
