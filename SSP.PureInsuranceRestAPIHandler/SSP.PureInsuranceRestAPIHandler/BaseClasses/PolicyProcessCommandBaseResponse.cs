namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PolicyProcessCommandBaseResponse : BaseResponseType
    {
        public AddPartyCommandResponse Insured { get; set; }
        public BaseNBQuoteResponseTypePolicy Policy { get; set; }
    }
}
