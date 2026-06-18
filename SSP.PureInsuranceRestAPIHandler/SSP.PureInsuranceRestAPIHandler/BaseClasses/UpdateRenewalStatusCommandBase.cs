using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRenewalStatusCommandBase : BaseRequestType
    {

        //[Range(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //[DefaultValue(0)]
        public int InsuranceFileKey { get; set; }
        public string QuoteTimeStamp { get; set; } 
        public string RenewalStatusCode { get; set; }
    }
}
