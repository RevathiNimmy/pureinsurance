using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailDetailsQueryBase : BaseRequestType
    {
        public DateTime FromDate { get; set; }
        public DateTime DateTo { get; set; }
        public int ModuleId { get; set; }
        public int UserId { get; set; }
    }
}
