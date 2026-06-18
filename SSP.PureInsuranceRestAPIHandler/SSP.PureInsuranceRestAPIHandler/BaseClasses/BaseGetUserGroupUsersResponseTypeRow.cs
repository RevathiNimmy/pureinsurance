namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUserGroupUsersResponseTypeRow
    {
        //[DBCol("EmailAddress")]
        public string EmailAddress { get; set; }
        //[DBCol("Name")]
        public string Name { get; set; }
        //[DBCol("UserKey")]
        public int UserKey { get; set; }
    }
}
