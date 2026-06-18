namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementsQueryBase : BaseRequestType
    {
		public int RIVersionID { get; set; }
        public int RiskKey { get; set; }
    }
    
}