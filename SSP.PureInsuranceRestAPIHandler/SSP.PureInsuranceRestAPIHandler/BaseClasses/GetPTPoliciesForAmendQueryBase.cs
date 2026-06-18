using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPTPoliciesForAmendQueryBase : BaseRequestType
    {
        public string PolicyNumber { get; set; }
    }
}
