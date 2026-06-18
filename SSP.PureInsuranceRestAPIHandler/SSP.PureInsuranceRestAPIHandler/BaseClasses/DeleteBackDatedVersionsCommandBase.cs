using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeleteBackDatedVersionsCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }
}
