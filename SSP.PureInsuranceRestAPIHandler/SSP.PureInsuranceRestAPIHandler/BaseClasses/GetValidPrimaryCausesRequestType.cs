using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetValidPrimaryCausesRequestType : BaseRequestType
    {
        public bool includeDeleted { get; set; }

        public int insuranceFileKey { get; set; }

        public int mode { get; set; }

        public bool modeSpecified { get; set; }
    }
}
