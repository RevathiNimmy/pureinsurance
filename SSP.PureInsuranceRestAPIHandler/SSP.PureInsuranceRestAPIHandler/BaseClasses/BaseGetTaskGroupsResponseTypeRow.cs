namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTaskGroupsResponseTypeRow
    {
        //[DBCol("CaptionID")]
        public int CaptionID { get; set; }
        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("IsDeleted")]
        public bool IsDeleted { get; set; }
        //[DBCol("TaskGroupCategoryKey")]
        public int TaskGroupCategoryKey { get; set; }
        //[DBCol("TaskGroupKey")]
        public int TaskGroupKey { get; set; }
    }
}
