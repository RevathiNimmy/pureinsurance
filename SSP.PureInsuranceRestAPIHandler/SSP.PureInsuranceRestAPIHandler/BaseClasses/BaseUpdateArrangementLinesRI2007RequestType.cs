using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateArrangementLinesRI2007RequestType : BaseRequestType
    {
        public System.Collections.Generic.List<BaseRiskRIArrangementLineType> RIArrangementLines { get; set; }
    }
}
