using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateTaskStatusCommandBase : BaseRequestType
    {
        public WMActionType? ActionType { get; set; }
        public int ExternalTaskStatus { get; set; }
        public string GuidPMExternalItem { get; set; }
        public int TaskInstanceKey { get; set; }
        public int TaskStatusKey { get; set; }
    }
}
