namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountingPeriod
{
    public class GetAccountingPeriodQuery : GetAccountingPeriodQueryBase //GetAccountingPeriodQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
