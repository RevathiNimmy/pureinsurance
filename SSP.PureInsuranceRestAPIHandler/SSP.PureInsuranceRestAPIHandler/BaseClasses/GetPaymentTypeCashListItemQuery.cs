namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentTypeCashListItem
{
    public class GetPaymentTypeCashListItemQuery : GetPaymentTypeCashListItemQueryBase //GetPaymentTypeCashListItemQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
    }
}
