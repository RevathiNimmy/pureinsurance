namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetailsEx
{
    public class GetTransactionDetailsExQuery : GetTransactionDetailsExQueryBase //GetTransactionDetailsExQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
