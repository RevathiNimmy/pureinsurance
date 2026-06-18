using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteAbandonClaimCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
    }
}
