using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddPartyBankDetailsCommandBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseAddPartyBankStatusType> PartyBankStatus { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
