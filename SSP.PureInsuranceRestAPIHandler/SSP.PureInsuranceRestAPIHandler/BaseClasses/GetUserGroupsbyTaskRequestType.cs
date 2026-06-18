using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsbyTaskRequestType : BaseRequestType
    {
        public string TaskGroupCode { get; set; }
    }
}
