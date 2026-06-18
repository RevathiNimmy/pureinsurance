using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetEventDetailsQueryResponse : BasePagedResponse
    {
        public List<BaseGetEventDetailsResponseTypeRow> EventDetails { get; set; }
    }
}
