using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateAgentCommissionCommandBase
    {
        public System.Collections.Generic.List<BaseUpdateAgentCommissionRequestTypeRow> AgentCommission { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
