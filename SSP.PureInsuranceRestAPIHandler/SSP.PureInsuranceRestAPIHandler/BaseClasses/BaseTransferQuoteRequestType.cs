namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTransferQuoteRequestType : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int PartyFromKey { get; set; }
        public int PartyToKey { get; set; }
    }
}
