using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddAgentReceiptCommandBase : BaseRequestType
    {
        [Newtonsoft.Json.JsonIgnore]
        public int PartyKey { get; set; }
        public BaseReceiptType Receipt { get; set; }
    }
}
