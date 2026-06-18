using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateRiskSelectionRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int IsSelected { get; set; }
        public int RiskKey { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
