namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindPolicyTransactionGrouped
{
    public class FindPolicyTransactionGroupedQuery : FindPolicyTransactionGroupedQueryBase //FindPolicyTransactionGroupedQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
