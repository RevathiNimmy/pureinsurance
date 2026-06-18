namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindPolicyTransactionGrouped
{
    public class FindPolicyTransactionGroupedQueryResponse : BasePagedResponse
    {
        public long ExecutionDuration { get; set; }
        public System.Collections.Generic.List<BaseFindPolicyTransactionGroupedResponseTypePolicies> Policies { get; set; }
    }
}
