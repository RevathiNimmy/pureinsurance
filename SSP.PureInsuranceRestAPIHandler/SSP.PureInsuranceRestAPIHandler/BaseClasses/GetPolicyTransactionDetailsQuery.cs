namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPolicyTransactionDetails
{
    public class GetPolicyTransactionDetailsQuery : GetPolicyTransactionDetailsQueryBase //GetPolicyTransactionDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
