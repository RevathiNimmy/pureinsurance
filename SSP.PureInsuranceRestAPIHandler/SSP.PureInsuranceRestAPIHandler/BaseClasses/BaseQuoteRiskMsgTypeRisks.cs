using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseQuoteRiskMsgTypeRisks : BaseRiskType
    {
        public System.Collections.Generic.List<BaseRiskRIArrangementType> RIArrangement { get; set; }
        public System.Collections.Generic.List<BaseRiskRatingSectionType> RatingSections { get; set; }
        public int SAMStagingRiskKey { get; set; }
        public int RiskFolderKey { get; set; }
        public int OriginalRiskKey { get; set; }
        public int RiskKey { get; set; }
        public bool RiskFolderKeySpecified { get; set; }
    }
}
