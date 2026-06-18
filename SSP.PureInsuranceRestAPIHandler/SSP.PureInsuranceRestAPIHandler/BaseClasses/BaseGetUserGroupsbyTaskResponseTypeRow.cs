namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserGroupsbyTaskResponseTypeRow
    {
        //[DBCol("UserGroupCode")]
        public string UserGroupCode { get; set; }
        //[DBCol("UserGroupDescription")]
        public string UserGroupDescription { get; set; }
        //[DBCol("UserGroupKey")]
        public int UserGroupKey { get; set; }
    }
}
