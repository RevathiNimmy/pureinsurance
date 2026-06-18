using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementLinesQueryResponse : BasePagedResponse
    {
        public List<BaseGetRiskReinsuranceArrangementLinesResponseTypeRow> ArrangementLines { get; set; }
    }
}