using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPartySummaryQueryBaseResponse : BasePagedResponse
    {
        public BasePartyType Item { get; set; }
        public byte[] PartyTimestamp { get; set; } = new byte[0];
        public System.Collections.Generic.List<BaseGetPartySummaryResponseTypeRow> Policies { get; set; }
    }
}
