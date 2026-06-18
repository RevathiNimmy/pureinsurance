using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPartyDetailsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimPartyDetailsResponseTypeRow> PartyDetails { get; set; }
    }
}
