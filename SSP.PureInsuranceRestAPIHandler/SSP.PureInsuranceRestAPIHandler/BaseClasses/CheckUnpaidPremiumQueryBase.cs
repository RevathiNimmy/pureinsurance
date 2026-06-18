using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckUnpaidPremiumQueryBase : BaseRequestType
    {
        public string ClaimNumber { get; set; }
        public string InsuranceRef { get; set; }
    }
}
