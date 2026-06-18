using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReinsuranceArrangementsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimReinsuranceArrangementsResponseTypeRow> ReinsuranceArrangements { get; set; }
    }
}
