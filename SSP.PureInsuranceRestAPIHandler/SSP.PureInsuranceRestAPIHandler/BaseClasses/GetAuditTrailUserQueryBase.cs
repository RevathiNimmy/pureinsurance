namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailUserQueryBase : BaseRequestType
    {
        public int UserId { get; set; }
        public bool UserIdSpecified { get; set; }
    }
}
