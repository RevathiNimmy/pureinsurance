using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdatePolicyPaymentMethodCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public string PolicyPaymentMethod { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
