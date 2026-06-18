namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPartyInsurerType : BaseClaimPartyType
    {
        public string ContactName { get; set; }
        public string InsurerShortName { get; set; }
        public string InsurerEmail { get; set; }
        public string InsurerFaxNo { get; set; }
        public string InsurerTelNo { get; set; }
        public string InsurerContact { get; set; }
    }
}
