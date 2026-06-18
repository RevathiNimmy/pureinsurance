namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddPartyCommandBaseResponse : BaseResponseType
    {
        public int PartyKey { get; set; }
        public byte[] PartyTimestamp { get; set; } = new byte[0];
        public string ResolvedName { get; set; }
        public string Shortname { get; set; }
        public string XMLDataset { get; set; }
    }
}
