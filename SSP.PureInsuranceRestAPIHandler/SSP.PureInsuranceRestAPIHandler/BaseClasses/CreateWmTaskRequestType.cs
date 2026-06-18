using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateWmTaskRequestType
    {
        public string AllocationUser { get; set; }
        public string AllocationUserGroup { get; set; }
        public string Client { get; set; }
        public string Description { get; set; }
        public System.DateTime DueDateTime { get; set; }
        public string ExternalTaskCategoryCode { get; set; }
        public int ExternalTaskStatus { get; set; }
        public string GuidPMExternalItem { get; set; }
        public bool IsComplete { get; set; }
        public bool IsExternalChildTask { get; set; }
        public bool IsExternalItem { get; set; }
        public bool IsTaskReview { get; set; }
        public bool IsUrgent { get; set; }
        public System.Collections.Generic.List<BaseCreateWmTaskRequestTypeRow> KeyData { get; set; }
        public TaskLockName LockName { get; set; }
        public int LockValue { get; set; }
        public int ParentTaskId { get; set; }
        public string Task { get; set; }
        public string TaskGroup { get; set; }
        public short UserId { get; set; }
        public string BranchCode { get; set; }
    }
}
