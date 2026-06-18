using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingDetailsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetRatingDetailsResponseTypeRow> RatingDetails { get; set; }
    }
}