using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CloneQuoteFromLivePolicyCommandBase : BaseRequestType
    {
        public bool CloneLivePolicy { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
