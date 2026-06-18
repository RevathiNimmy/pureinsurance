using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateClaimRIArrangementLinesRI2007CommandBase : BaseRequestType
    {
        public List<BaseClaimRiskRIArrangementLineType> ClaimRIArrangementLines { get; set; }
    }
}
