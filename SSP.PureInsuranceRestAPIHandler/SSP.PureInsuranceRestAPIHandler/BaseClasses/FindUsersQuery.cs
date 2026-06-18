namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindUsersQuery : FindUsersQueryBase
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public string SortBy { get; set; }
    }
}
