using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class LapseRenewalCommandBaseResponse : BaseResponseType
    {
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
    }
}
