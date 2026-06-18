
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupTaskGroupsQueryBase : BaseRequestType
    {
        public DateTime EffectiveDate { get; set; }
        public string UserGroupCode { get; set; }
    }
}
