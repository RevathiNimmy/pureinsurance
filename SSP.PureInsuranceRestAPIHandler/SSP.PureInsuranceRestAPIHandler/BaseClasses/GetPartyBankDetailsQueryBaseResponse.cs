using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPartyBankDetailsQueryBaseResponse : BasePagedResponse
    {
        public bool IsExternalCreditCardHandling { get; set; }
        public int LastTransactedPartyBankKey { get; set; }
        public System.Collections.Generic.List<BasePartyBankType> PartyBankDetails { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
