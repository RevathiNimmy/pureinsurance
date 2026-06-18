namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateRiskStatus
    {
        public int InsuranceFileKey { get; set; }
        public System.DateTime RiskInceptionDate { get; set; }
        public bool RiskInceptionDateSpecified { get; set; }
        public int RiskKey { get; set; }
        public string RiskStatusCode { get; set; }
    }
}
