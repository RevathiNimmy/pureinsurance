namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetReceiptCashListItems
{
    public class GetReceiptCashListItemsQuery : GetReceiptCashListItemsQueryBase //GetReceiptCashListItemsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
