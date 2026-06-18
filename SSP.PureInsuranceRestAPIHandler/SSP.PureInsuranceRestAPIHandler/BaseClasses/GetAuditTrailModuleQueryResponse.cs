using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailModuleQueryResponse : BasePagedResponse
    {
        public List<BaseGetAuditTrailModuleResponseType> AuditTrailModule { get; set; }
    }
}
