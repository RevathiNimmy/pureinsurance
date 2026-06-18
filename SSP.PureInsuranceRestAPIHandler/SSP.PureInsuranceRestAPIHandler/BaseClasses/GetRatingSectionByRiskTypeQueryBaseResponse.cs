using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingSectionByRiskTypeQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetRatingSectionByRiskTypeResponseTypeRow> RatingSections { get; set; }
    }
}
