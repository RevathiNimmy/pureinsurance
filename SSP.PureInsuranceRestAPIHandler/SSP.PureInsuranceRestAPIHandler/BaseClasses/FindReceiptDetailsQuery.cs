namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindReceiptDetails
{
    public class FindReceiptDetailsQuery : FindReceiptDetailsQueryBase //FindReceiptDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
