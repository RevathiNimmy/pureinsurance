using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductByAgentQueryBase : BaseRequestType
    {
        public int AgentKey { get; set; }
    }
}
