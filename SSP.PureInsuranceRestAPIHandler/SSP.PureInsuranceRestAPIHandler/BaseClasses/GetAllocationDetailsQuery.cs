namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAllocationDetails
{
    public class GetAllocationDetailsQuery : GetAllocationDetailsQueryBase //GetAllocationDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
