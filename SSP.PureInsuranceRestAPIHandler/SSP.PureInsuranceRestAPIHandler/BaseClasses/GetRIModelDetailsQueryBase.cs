using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRIModelDetailsQueryBase :BaseRequestType
    {
        public string RIModelCode { get; set; }
    }
}
