
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsbyTaskQueryBase : BaseRequestType
    {
        public string TaskGroupCode { get; set; }
    }
}
