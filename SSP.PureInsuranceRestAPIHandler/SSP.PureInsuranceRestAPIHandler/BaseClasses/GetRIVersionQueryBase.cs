using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIVersionQueryBase :  BaseRequestType
    {
        public int RiskCnt { get; set; }
    }
}
