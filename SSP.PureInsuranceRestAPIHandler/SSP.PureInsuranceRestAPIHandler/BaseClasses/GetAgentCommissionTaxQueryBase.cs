namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentCommissionTaxQueryBase : BaseRequestType
    {
        public double AgentCommissionAmount { get; set; }

        public string CurrencyCode { get; set; }

        public int InsuranceFileKey { get; set; }

        public string TaxGroupCode { get; set; }
    }
}
