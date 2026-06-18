using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailDetailsQueryResponse : BasePagedResponse
    {
        public List<BaseGetAuditTrailResponseType> AuditTrails { get; set; }
    }
}
