namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskAdditionalDetailsType
    {
        public int RiskKey { get; set; }
        public string RiskTypeDescription { get; set; }
        public bool PostTaxEntriesSeparately { get; set; }
    }
}
