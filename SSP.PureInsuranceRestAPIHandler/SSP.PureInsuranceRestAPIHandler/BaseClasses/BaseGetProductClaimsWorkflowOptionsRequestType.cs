using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductClaimsWorkflowOptionsRequestType : BaseRequestType
    {
        /// <remarks/>
        public int ProductID { get; set; }

        /// <remarks/>
        public string ProductCode { get; set; }

        /// <remarks/>
        public ClaimProcessType ClaimProcessType { get; set; }


    }

}
