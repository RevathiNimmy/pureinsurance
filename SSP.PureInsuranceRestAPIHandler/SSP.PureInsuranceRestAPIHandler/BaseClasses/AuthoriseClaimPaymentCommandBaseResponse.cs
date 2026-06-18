using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AuthoriseClaimPaymentCommandBaseResponse : BaseResponseType
    {
        public bool AllocationStatus { get; set; }
        public string ErrorMessage { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
