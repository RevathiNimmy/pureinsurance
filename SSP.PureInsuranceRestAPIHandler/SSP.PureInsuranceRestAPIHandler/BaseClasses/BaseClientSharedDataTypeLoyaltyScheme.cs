namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClientSharedDataTypeLoyaltyScheme
    {
        public int LoyaltySchemeKey { get; set; }
        public string LoyaltySchemeCode { get; set; }
        public string MembershipNumber { get; set; }
        public string OtherReference { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool EndDateSpecified { get; set; }
        public string MainMember { get; set; }
        public bool Active { get; set; }
        public bool ActiveSpecified { get; set; }
        public int ProcessingStatus { get; set; }

    }
}
