using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyStatusForMediaTypeStatusResponseType : BaseResponseType
    {

        public bool IsClaimPaymentInitiated { get; set; }
        
        public bool IsClaimPaymentInitiatedOnLossDate { get; set; }

        public int IsPolicyCanceled { get; set; } 

        public bool IsUnclearedCashListExists { get; set; }

    }
}
