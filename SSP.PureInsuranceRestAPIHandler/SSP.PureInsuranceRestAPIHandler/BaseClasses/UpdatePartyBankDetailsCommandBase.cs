using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePartyBankDetailsCommandBase : BaseRequestType
    {
        public System.Collections.Generic.List<BasePartyBankType> PartyBankDetails { get; set; }
        public int PartyKey { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
