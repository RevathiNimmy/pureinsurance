using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTreatyPartyDetailsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetTreatyPartyDetailsResponseTypePartiesRow> Parties { get; set; }
    }
}
