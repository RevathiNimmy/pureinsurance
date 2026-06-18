using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndRiskFeesByKeyQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int RiskKey { get; set; }
    }
}
