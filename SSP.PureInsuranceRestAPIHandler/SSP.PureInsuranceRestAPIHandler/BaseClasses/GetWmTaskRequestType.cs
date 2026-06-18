using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWmTaskRequestType : BaseRequestType
    {
        public int TaskInstanceKey { get; set; }
    }
}
