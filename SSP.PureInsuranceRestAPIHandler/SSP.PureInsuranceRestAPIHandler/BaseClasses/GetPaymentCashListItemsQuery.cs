namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentCashListItems
{
    public class GetPaymentCashListItemsQuery : GetPaymentCashListItemsQueryBase //GetPaymentCashListItemsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
