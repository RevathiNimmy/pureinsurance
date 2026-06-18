using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ReAssignMultipleWmTasksCommandBase : BaseRequestType
    {
        public List<BaseReAssignMultipleWmTasksRequestTypeRow> Tasks { get; set; }
    }
}
