namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentCashListItemDetails
{
    public class GetPaymentCashListItemDetailsQuery : GetPaymentCashListItemDetailsQueryBase //GetPaymentCashListItemDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
