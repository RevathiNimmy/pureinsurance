namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUserDetailsType
    {
        public System.DateTime EffectiveDate { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public int UserKey { get; set; }
        public string UserName { get; set; }
    }
}
