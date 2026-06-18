
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetVersionsForClaimQueryBase : BaseRequestType
    {
        public string ClaimNumber { get; set; }
        
        public int BaseClaimKey { get; set; }
        
        public int ClaimId { get; set; }
    }
}
