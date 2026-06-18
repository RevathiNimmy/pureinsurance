namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPartyPoliciesQueryBase : BaseRequestType
    {
        public string PartyCode { get; set; }
        public bool RetrieveAssociates { get; set; }
        public int AgentKey { get; set; }
    }
}
