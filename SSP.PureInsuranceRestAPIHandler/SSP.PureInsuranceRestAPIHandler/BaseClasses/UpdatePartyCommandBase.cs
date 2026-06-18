namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePartyCommandBase : BaseRequestType
    {
        public BasePartyPCType BasePartyPCType { get; set; } = null;
        public BasePartyCCType BasePartyCCType { get; set; } = null;
        public BasePartyOtherType BasePartyOTHERType { get; set; } = null;
        public int PartyKey { get; set; }
        public byte[] PartyTimestamp { get; set; } = new byte[0];
        public string SubBranchCode { get; set; }
    }
}
