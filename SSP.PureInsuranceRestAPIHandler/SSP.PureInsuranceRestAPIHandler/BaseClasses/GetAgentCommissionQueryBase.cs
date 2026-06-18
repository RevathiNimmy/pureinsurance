namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentCommissionQueryBase : BaseRequestType
    {
        public string CommissionBand { get; set; }

        public int InsuranceFileKey { get; set; }
        public string RiskType { get; set; }
    }
}
