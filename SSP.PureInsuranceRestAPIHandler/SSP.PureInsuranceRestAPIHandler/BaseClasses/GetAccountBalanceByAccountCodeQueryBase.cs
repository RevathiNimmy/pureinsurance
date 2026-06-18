using Newtonsoft.Json;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountBalanceByAccountCode
{
    public class GetAccountBalanceByAccountCodeQueryBase : BaseRequestType
    {
        
        public string AccountCode { get; set; }
        public bool RestrictToNonPolicyTransactions { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public int AgentKey { get; set; }
    }
}
