namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserDetailsResponseTypeRow
    {
        //[DBCol("Code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("IsAssociated")]
        public int IsAssociated { get; set; }
        //[DBCol("IsSupervisor")]
        public int IsSupervisor { get; set; }
        //[DBCol("IsSystemAdmin")]
        public int IsSystemAdmin { get; set; }
        //[DBCol("UserGroupKey")]
        public int UserGroupKey { get; set; }
    }
}
