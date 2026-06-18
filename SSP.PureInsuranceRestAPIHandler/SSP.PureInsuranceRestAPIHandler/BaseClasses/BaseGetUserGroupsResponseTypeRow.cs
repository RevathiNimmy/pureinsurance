namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserGroupsResponseTypeRow
    {

        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("EffectiveDate")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("IsDebtorPMUserGroup")]
        public bool IsDebtorPMUserGroup { get; set; }
        //[DBCol("IsDeleted")]
        public bool IsDeleted { get; set; }
        //[DBCol("IsSystemAdmin")]
        public bool IsSystemAdmin { get; set; }
        //[DBCol("UserGroupKey")]
        public int UserGroupKey { get; set; }
    }
}
