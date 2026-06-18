using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateClaimRiskRequestType : BaseRequestType
    {
        public int BaseClaimKey { get; set; }

        public byte[] TimeStamp { get; set; }

        public string XMLDataSet{ get; set; }

    }
}
