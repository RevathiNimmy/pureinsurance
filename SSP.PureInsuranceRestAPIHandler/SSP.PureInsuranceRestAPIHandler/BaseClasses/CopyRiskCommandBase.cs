using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CopyRiskCommandBase : BaseRequestType
    {
        public CopyRiskType? CopyType { get; set; }
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int RiskKey { get; set; }
        public int RiskNumber { get; set; }
    }
}
