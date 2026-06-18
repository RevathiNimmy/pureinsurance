using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAgentCommissionResponseType : BasePagedResponse 
    {
        public List<BaseAgentCommissionResponseTypeRow> AgentCommission { get; set; }
        public double LeadAgentNet { get; set; }
        public double LeadAgentTotalCommission { get; set; }
        public double LeadAgentTotalTax { get; set; }
        public double SubAgentNet { get; set; }
        public double SubAgentTotalCommission { get; set; }
        public double SubAgentTotalTax { get; set; }
    }
}
