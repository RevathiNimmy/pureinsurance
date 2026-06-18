using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateTaskGroupsCommandBase : BaseRequestType
    {
        public int CaptionId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public int TaskGroupCategoryKey { get; set; }
        public int TaskGroupKey { get; set; }
    }
}
