namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyOtherTypeAccident
    {
        public int AccidentKey { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAtFault { get; set; }
        public int ProcessingStatus { get; set; }
    }
}
