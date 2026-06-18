using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddUserGroupCommandBase : BaseRequestType
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSysAdmin { get; set; }
    }
}
