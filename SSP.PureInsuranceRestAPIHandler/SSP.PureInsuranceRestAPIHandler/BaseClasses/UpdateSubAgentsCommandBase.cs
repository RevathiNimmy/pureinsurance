using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateSubAgentsCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public List<BaseUpdateSubAgentsRequestTypeSubAgentsRow> SubAgents { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
