using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class MaintainClaimRequestType : BaseRequestType
    {
        public BaseClaimMaintainType claim { get; set; }

        public byte[] timeStamp { get; set; }
    }
}
