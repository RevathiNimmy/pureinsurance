
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class MaintainClaimCommandBase : BaseRequestType
    {
        public BaseClaimMaintainType Claim { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        
        public bool DataTransferClaimHasSpecifiedReinsurance { get; set; }
        
        public bool DataTransferIsUsingFullClaimVersioning { get; set; }
        
        public bool IsDataTransferClaim { get; set; }
        
        public bool DataTransferClaimHasClaimRiskDataSpecified { get; set; }
    }
}
