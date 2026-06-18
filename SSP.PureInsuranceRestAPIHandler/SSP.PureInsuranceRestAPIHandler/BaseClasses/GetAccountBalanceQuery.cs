namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountBalance
{
    public class GetAccountBalanceQuery : GetAccountBalanceQueryBase //GetAccountBalanceQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
