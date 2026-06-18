namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindPaymentDetails
{
    public class FindPaymentDetailsQuery : FindPaymentDetailsQueryBase //FindPaymentDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
