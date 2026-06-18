using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAuditTrailUserQueryResponse : BasePagedResponse
    {
        public List<BaseGetAuditTrailUserResponseType> AuditTrailUser { get; set; }
    }
}
