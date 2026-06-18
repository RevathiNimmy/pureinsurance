namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAuditTrailResponseType
    {
        public int ConfigurationAuditdetailId { get; set; }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public System.DateTime EventFromDate { get; set; }

        public System.DateTime EventToDate { get; set; }
        public string ScreenDescription { get; set; }
        public string FieldDescription { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    }
}
