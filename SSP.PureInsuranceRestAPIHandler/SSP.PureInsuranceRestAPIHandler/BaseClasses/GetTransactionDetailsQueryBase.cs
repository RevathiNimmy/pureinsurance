namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetails
{
    public class GetTransactionDetailsQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public System.DateTime AccountingDate { get; set; }
        public System.Collections.Generic.List<BaseGetTransactionDetailsRequestTypeRow> Allocation { get; set; }
        public bool IncludeReversedTran { get; set; }
        public string InsuranceRef { get; set; }
        public bool IsNewPF { get; set; }
        public bool IsOutstandingOnly { get; set; }
        public string ShortCode { get; set; } = string.Empty;
        
        [Newtonsoft.Json.JsonIgnore]
        public int AgentKey { get; set; }
    }
}
