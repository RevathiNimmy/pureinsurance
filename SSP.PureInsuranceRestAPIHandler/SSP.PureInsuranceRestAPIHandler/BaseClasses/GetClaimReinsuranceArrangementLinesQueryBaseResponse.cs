using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReinsuranceArrangementLinesQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimReinsuranceArrangementLinesResponseTypeRow> ReinsuranceArrangementLines { get; set; }
    }
}
