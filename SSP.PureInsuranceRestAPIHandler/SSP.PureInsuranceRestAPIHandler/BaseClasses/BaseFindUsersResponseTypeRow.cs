namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindUsersResponseTypeRow
    {
        //[DBCol("effective_date")]
        public System.DateTime EffectiveDate { get; set; }
        //[DBCol("full_name")]
        public string FullName { get; set; }
        //[DBCol("user_id")]
        public int UserId { get; set; }
        //[DBCol("Username")]
        public string UserName { get; set; }
    }
}
