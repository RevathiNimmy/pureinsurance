namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetTaskGroupTasksResponseTypeRow
    {
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("DisplayIcon")]
        public int DisplayIcon { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("IsAvailable")]
        public bool IsAvailable { get; set; }
        //[DBCol("IsDeleted")]
        public bool IsDeleted { get; set; }
        //[DBCol("IsIncluded")]
        public bool IsIncluded { get; set; }
        //[DBCol("IsViewOnly")]
        public bool IsViewOnly { get; set; }
        //[DBCol("Name")]
        public string Name { get; set; }
        //[DBCol("TaskCategoryKey")]
        public int TaskCategoryKey { get; set; }
        //[DBCol("TaskKey")]
        public int TaskKey { get; set; }
    }
}
