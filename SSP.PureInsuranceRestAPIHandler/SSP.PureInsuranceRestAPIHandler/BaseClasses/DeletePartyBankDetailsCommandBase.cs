using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeletePartyBankDetailsCommandBase : BaseRequestType
    {
        public System.Collections.Generic.List<BaseDeletePartyBankDetailsRequestTypeRow> PartBankDetails { get; set; }
        public int PartyKey { get; set; }
    }
}
