namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindPoliciesByRiskindexQueryBase : BaseRequestType
    {
		public int PartyKey { get; set; }
        public string RiskIndex { get; set; }
    }
    
}