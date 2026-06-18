namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaskGroupsQuery : GetTaskGroupsQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
