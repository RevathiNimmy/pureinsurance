namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddClientDataExtractAuditTrailCommandBase : BaseRequestType
    {
        public int PartyKey { get; set; }
        public string ClientCode { get; set; }
    }
}
