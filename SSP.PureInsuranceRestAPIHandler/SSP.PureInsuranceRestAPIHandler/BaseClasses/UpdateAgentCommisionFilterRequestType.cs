namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateAgentCommisionFilterRequestType
    {
        public int RiskTypeId { get; set; }
        public int CommissionBankId { get; set; }
        public int TaxGroupId { get; set; }
        public int PartyId { get; set; }
    }
}
