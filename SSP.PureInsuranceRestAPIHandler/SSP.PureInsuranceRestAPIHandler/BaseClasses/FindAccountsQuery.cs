namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindAccountsQuery
{
    public class FindAccountsQuery : FindAccountsQueryBase //FindAccountsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
