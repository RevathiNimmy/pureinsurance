using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskOnKeysRequestType :BaseRequestType
    {
        public BaseCreateWmTaskRequestTypeKeyData KeyData { get; set; }
    }
}
