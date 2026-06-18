using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseActivatePartyBankRequestType : BaseRequestType
    {
        public System.Collections.Generic.List<BaseActivatePartyBankRequestTypeRow> PartBankDetails { get; set; }
        public int PartyKey { get; set; }
    }
}
