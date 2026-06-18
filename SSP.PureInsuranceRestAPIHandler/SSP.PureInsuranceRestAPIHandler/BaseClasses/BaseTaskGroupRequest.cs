namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTaskGroupRequest
    {
        public int CaptionId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public int TaskGroupCategoryKey { get; set; }
    }
}
