namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserGroupsQuery : GetUserGroupsQueryBase
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
    }
}
