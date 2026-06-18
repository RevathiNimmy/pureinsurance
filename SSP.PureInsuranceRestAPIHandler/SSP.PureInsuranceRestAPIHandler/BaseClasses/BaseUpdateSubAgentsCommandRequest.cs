using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateSubAgentsCommandRequest : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public System.Collections.Generic.List<BaseUpdateSubAgentsRequestTypeSubAgentsRow> SubAgents { get; set; }
    }
}
