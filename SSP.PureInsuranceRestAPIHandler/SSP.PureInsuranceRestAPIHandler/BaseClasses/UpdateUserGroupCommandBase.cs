using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateUserGroupCommandBase : BaseRequestType
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSysAdmin { get; set; }
        public int UserGroupKey { get; set; }
    }
}
