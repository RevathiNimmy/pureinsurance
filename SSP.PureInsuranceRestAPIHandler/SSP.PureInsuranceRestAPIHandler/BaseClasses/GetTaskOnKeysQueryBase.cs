
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskOnKeysQueryBase : BaseRequestType
    {
        public BaseCreateWmTaskRequestTypeKeyData KeyData { get; set; }
        public int AgentKey { get; set; }
    }
}
