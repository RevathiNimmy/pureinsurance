using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseDeleteAbandonClaimRequestType : BaseRequestType
    {
        public int ClaimKey { get; set; }

        public byte[] TimeStamp { get; set; }
    }
}
