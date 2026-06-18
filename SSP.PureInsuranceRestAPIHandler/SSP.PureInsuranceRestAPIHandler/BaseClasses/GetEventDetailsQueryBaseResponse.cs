using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetEventDetailsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetEventDetailsResponseTypeRow> EventDetails { get; set; }
    }
}
