namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetWmTaskLogResponseTypeTaskLogRow
    {
        //[DBCol("created_by_id")]
        public int CreatedByKey { get; set; }
        //[DBCol("date_created")]
        public System.DateTime DateCreated { get; set; }
        //[DBCol("text")]
        public string LogText { get; set; }
        //[DBCol("pmwrk_task_instance_cnt")]
        public int TaskInstanceKey { get; set; }
        //[DBCol("username")]
        public string UserName { get; set; }
    }
}
