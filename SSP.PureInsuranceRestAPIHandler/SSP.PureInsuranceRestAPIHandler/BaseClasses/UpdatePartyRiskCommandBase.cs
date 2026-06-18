namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePartyRiskCommandBase : BaseRequestType
    {
        public int PartyKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public string XMLDataSet { get; set; }
    }
}
