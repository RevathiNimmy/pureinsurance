using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClonePoliciesForAmendQueryBase : BaseRequestType
    {
        public string PolicyNumber { get; set; }
    }
}
