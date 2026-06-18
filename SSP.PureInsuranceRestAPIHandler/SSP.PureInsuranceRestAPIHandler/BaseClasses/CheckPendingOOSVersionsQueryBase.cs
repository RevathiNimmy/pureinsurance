using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckPendingOOSVersionsQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
    }
}
