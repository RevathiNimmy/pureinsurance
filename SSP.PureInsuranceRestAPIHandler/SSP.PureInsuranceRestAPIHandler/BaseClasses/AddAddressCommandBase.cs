using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddAddressCommandBase : BaseAddressType
    {
        public string  BranchCode { get; set; }
       
        public string LoginUserName { get; set; }
       
        public string WCFSecurityToken { get; set; }
    }
}
