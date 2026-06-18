
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWmTaskLogQueryBase : BaseRequestType
    {
        public int TaskInstanceKey { get; set; }
    }
}
