namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentCommissionTaxQueryBaseResponse : BaseResponseType
    {
        public double TaxBaseAmount { get; set; }
        public double TaxCurrencyAmount { get; set; }
    }
}
