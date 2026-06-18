namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailModuleQueryBase : BaseRequestType
    {
        public int ModuleId { get; set; }
        public bool ModuleIdSpecified { get; set; }
    }
}
