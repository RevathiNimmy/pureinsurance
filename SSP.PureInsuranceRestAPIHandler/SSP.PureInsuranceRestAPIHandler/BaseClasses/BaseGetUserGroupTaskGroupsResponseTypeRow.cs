namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserGroupTaskGroupsResponseTypeRow
    {
        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("DisplaySequence")]
        public int DisplaySequence { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("IsDeleted")]
        public bool IsDeleted { get; set; }
        //[DBCol("IsIncluded")]
        public bool IsIncluded { get; set; }
        //[DBCol("TaskGroupKey")]
        public int TaskGroupKey { get; set; }
    }
}
