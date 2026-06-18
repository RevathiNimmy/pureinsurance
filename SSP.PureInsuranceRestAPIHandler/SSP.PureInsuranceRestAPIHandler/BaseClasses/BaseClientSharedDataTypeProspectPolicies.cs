namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClientSharedDataTypeProspectPolicies
    {
        public int ProspectPolicyKey { get; set; }
        public string ProspectTypeCode { get; set; }
        public System.DateTime RenewalDate { get; set; }
        public bool RenewalDateSpecified { get; set; }
        public decimal TimesQuoted { get; set; }
        public bool TimesQuotedSpecified { get; set; }
        public decimal TargetPremium { get; set; }
        public bool TargetPremiumSpecified { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
