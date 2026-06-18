using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingDetailsQueryBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseGetRatingDetailsResponseTypeRow> RatingDetails { get; set; }
    }
}
