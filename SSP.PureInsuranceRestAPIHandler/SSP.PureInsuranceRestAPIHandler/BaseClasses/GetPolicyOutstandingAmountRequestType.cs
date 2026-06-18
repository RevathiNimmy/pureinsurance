using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyOutstandingAmountRequestType : BaseRequestType
    {
        public int InsuarnaceFilecnt { get; set; }
    }
}
