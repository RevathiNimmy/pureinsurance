namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBalancesAndUnallocatedCredits
{
    public class GetBalancesAndUnallocatedCreditsQueryBaseResponse : BaseResponseType
    {
        public double AccountBalance { get; set; }
        public int AgentKey { get; set; }
        public string AgentType { get; set; }
        public int ClientKey { get; set; }
        public double FloatBalanceLimit { get; set; }
        public string InsuranceRef { get; set; }
        public bool IsFloatBalanceAccount { get; set; }
        public bool IsOverDraftAccount { get; set; }
        public System.DateTime OverDraftExpiry { get; set; }
        public double OverDraftLimit { get; set; }
        public System.Collections.Generic.List<BaseGetBalancesAndUnallocatedCreditsResponseTypeRow> UnallocatedCreditsForAgents { get; set; }
        public System.Collections.Generic.List<BaseGetBalancesAndUnallocatedCreditsResponseTypeRow1> UnallocatedCreditsForClients { get; set; }
    }
}
