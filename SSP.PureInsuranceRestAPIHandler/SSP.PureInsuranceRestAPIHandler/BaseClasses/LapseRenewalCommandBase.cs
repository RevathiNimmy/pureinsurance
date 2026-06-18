using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class LapseRenewalCommandBase  : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public string LapseReasonCode { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
