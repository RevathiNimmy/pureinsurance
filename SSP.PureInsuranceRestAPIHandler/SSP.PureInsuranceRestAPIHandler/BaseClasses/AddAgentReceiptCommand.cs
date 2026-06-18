
using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddAgentReceiptCommand : AddAgentReceiptCommandBase
    {
        [Newtonsoft.Json.JsonIgnore]
        public int SourceKey { get; set; }
    }
}
