using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CancelPremiumFinancePlanCommandBase : BaseRequestType
    {
        public int PFPremiumFinanceKey { get; set; }
        public int PFPremiumFinanceVersionKey { get; set; }
        public string ReasonCode { get; set; }
        public CancelPFPlanType? RequestType { get; set; }
    }
}
