namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetails
{
    public class GetTransactionDetailsQuery : GetTransactionDetailsQueryBase //GetTransactionDetailsQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
    }
}
