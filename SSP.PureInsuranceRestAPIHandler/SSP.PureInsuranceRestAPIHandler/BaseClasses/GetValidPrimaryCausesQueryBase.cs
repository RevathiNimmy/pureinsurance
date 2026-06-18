
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetValidPrimaryCausesQueryBase : BaseRequestType
    {
        public bool IncludeDeleted { get; set; }
        public int InsuranceFileKey { get; set; }
        public int Mode { get; set; }
        public bool ModeSpecified { get; set; }
    }
}
