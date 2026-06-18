namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetWorkManagerScheduledTasksResponseTypeTasksRow
    {
        //[DBCol("Customer")]
        public string Customer { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("DueDate")]
        public System.DateTime DueDate { get; set; }
        //[DBCol("GuidPMExternalItem")]
        public System.Guid GuidPMExternalItem { get; set; }

        //[DBCol("IsExternalItem")]
        public bool IsExternalItem { get; set; }
        //[DBCol("ParentTaskKey")]
        public int ParentTaskKey { get; set; }
        //[DBCol("PartyCnt")]
        public int PartyKey { get; set; }
        //[DBCol("PartyName")]
        public string PartyName { get; set; }
        //[DBCol("TaskGroupKey")]
        public int TaskGroupKey { get; set; }
        //[DBCol("TaskInstanceKey")]
        public int TaskInstanceKey { get; set; }
        //[DBCol("TaskKey")]
        public int TaskKey { get; set; }
        //[DBCol("TaskStatusKey")]
        public int TaskStatusKey { get; set; }
        //[DBCol("Type")]
        public string Type { get; set; }
        //[DBCol("Urgent")]
        public int Urgent { get; set; }
        //[DBCol("UserCode")]
        public string UserCode { get; set; }
        //[DBCol("UserGroupCode")]
        public string UserGroupCode { get; set; }
        //[DBCol("UserGroupDescription")]
        public string UserGroupDescription { get; set; }
        //[DBCol("UserGroupKey")]
        public int UserGroupKey { get; set; }
        //[DBCol("UserKey")]
        public int UserKey { get; set; }
    }
}
