namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesByRefQueryBase : BaseRequestType
    {
        public string InsuranceRef { get; set; }
        public bool SkipPolicyLevelTaxesRecalculation { get; set; }
    }
}
