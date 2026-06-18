namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetReceiptCashListDetails
{
    public class GetReceiptCashListDetailsQuery : GetReceiptCashListDetailsQueryBase //GetReceiptCashListDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
