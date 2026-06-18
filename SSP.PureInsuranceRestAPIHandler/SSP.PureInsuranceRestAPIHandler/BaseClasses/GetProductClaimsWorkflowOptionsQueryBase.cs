using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductClaimsWorkflowOptionsQueryBase : BaseRequestType
    {
        public ClaimProcessType ClaimProcessType { get; set; }
        public string ProductCode { get; set; }
    }
}
