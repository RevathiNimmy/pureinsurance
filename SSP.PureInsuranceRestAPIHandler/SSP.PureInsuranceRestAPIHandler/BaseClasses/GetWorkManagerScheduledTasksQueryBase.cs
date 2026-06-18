using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWorkManagerScheduledTasksQueryBase : BaseRequestType
    {
        public DateRange? Date { get; set; }
        public bool DateSpecified { get; set; }
        public int PartyKey { get; set; }
        public string ReferenceNumber { get; set; }
        public ShowType? ShowSystemKEY { get; set; }
        public bool ShowSystemKEYSpecified { get; set; }
        public TaskStatus? TaskStatusKey { get; set; }
        public bool TaskStatusKeySpecified { get; set; }
        public string UserCODE { get; set; }
        public string UserGroupCODE { get; set; }
        public int AgentKey { get; set; }
    }
}
