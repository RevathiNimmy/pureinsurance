using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRIArrangementLinesRI2007QueryResponse : BasePagedResponse
    {
        public List<BaseClaimRiskRIArrangementLineType> RIArrangementLines { get; set; }
    }
}
