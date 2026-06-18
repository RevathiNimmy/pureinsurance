namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceBandsQueryBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }

        public int RiskKey { get; set; }
    }
}
