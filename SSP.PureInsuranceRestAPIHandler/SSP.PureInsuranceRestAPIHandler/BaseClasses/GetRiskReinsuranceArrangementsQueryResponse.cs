using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementsQueryResponse : BasePagedResponse
    {
        public List<BaseGetRiskReinsuranceArrangementsResponseTypeRow> Arrangements { get; set; }
    }
}