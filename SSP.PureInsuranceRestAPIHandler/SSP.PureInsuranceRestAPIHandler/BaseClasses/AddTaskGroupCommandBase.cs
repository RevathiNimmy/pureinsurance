using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddTaskGroupCommandBase : BaseRequestType
    {
        public int CaptionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public int TaskGroupCategoryKey { get; set; }
    }
}
