using SSP.PureInsuranceRestAPIHandler.Enums;
using System;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateWmTaskCommandBase : BaseRequestType
    {
        public string AllocationUser { get; set; }
        public string AllocationUserGroup { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDateTime { get; set; }
        public string ExternalTaskCategoryCode { get; set; } = string.Empty;
        public int ExternalTaskStatus { get; set; }
        public string GuidPMExternalItem { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public bool IsExternalChildTask { get; set; }
        public bool IsExternalItem { get; set; }
        public bool IsTaskReview { get; set; }
        public bool IsUrgent { get; set; }
        public List<BaseCreateWmTaskRequestTypeRow> KeyData { get; set; }
        public TaskLockName? LockName { get; set; }
        public int LockValue { get; set; }
        public int ParentTaskId { get; set; }
        public string Task { get; set; } = string.Empty;
        public string TaskGroup { get; set; } = string.Empty;
    }
}
