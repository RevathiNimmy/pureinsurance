using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePartyBankDetailsCommandBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseUpdatePartyBankStatusType> PartyBankStatus { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
