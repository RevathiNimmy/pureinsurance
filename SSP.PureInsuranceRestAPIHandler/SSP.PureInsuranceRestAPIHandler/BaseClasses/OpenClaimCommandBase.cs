
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class OpenClaimCommandBase : BaseRequestType
    {
        public BaseClaimOpenType Claim { get; set; }
        
        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }
        
        public bool DataTransferIsUsingFullClaimVersioning { get; set; }
        
        public bool IsDataTransferClaim { get; set; }
        
        public bool IsRoundingUpToFour { get; set; }
    }
}
