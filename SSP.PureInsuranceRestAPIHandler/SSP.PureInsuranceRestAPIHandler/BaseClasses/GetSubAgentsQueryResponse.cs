using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetSubAgentsQueryResponse : BasePagedResponse
    {
        public List<BaseGetSubAgentsResponseTypeRow> SubAgents { get; set; }
    }
}
