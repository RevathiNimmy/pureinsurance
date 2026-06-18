namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetInsurerPayments
{
    public class GetInsurerPaymentsQuery : GetInsurerPaymentsQueryBase //GetInsurerPaymentsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
