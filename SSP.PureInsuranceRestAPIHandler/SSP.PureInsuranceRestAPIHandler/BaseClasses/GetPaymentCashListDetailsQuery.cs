namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPaymentCashListDetails
{
    public class GetPaymentCashListDetailsQuery : GetPaymentCashListDetailsQueryBase //GetPaymentCashListDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
