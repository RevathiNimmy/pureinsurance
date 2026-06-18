
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWmTaskQueryBaseResponse : BasePagedResponse
    {
        public int CreatedByKey { get; set; }
        public bool CreatedByKeySpecified { get; set; }
        public string CreatedByUser { get; set; }
        public string Customer { get; set; }
        public DateTime DateCreated { get; set; }
        public bool DateCreatedSpecified { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int IsTaskReview { get; set; }
        public bool IsTaskReviewSpecified { get; set; }
        public int IsUrgent { get; set; }
        public bool IsUrgentSpecified { get; set; }
        public int Isvisible { get; set; }
        public bool IsvisibleSpecified { get; set; }
        public List<BaseGetWmTaskResponseTypeRow> KeyData { get; set; }
        public DateTime LastModified { get; set; }
        public bool LastModifiedSpecified { get; set; }
        public int ModifiedByKey { get; set; }
        public bool ModifiedByKeySpecified { get; set; }
        public string ModifiedByUser { get; set; }
        public string TaskCode { get; set; }
        public string TaskGroupCode { get; set; }
        public int TaskGroupKey { get; set; }
        public int TaskInstanceKey { get; set; }
        public int TaskKey { get; set; }
        public int TaskStatusKey { get; set; }
        public bool TaskStatusKeySpecified { get; set; }
        public byte[] TaskTimestamp { get; set; } = new byte[0];
        public string UserCode { get; set; }
        public string UserGroupCode { get; set; }
        public int UserGroupKey { get; set; }
        public int UserKey { get; set; }
        public string WorkflowInformation { get; set; }
    }
}
