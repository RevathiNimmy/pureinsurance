using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAuthoriseClaimPaymentResponseType : BaseResponseType
    {
        public Boolean AllocationStatus { get; set; }

        public String ErrorMessage { get; set; }

        public Byte[] TimeStamp { get; set; }
    }
}
