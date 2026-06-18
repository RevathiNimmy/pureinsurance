using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceBandsQueryResponse : GetRiskReinsuranceBandsQueryBaseResponse
    {
        public List<BaseGetRiskReinsuranceBandsResponseTypeRow> ReinsuranceBands { get; set; }
    }
}