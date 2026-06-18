using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateAgentCommissionRequestType : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public System.Collections.Generic.List<BaseUpdateAgentCommissionRequestTypeRow> AgentCommission { get; set; }
    }
}
