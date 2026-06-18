using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskByProductQueryBase : BaseRequestType
    {
        public string ProductCode { get; set; }
    }
}
