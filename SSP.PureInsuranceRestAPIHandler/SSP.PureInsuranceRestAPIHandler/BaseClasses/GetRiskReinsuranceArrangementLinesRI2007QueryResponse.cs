using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementLinesRI2007QueryResponse : BasePagedResponse
    {
       public List<BaseRiskRIArrangementLineType> ArrangementLines { get; set; }
    }
}