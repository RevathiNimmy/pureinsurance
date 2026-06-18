namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPartySummaryQueryBase : BaseRequestType
    {
        public int PartyKey { get; set; }
        public bool RetrieveAssociates { get; set; }
    }
}
