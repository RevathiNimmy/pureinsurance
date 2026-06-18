namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetBankAccounts
{
    public class GetBankAccountsQuery : GetBankAccountsQueryBase //GetBankAccountsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
