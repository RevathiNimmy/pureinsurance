namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetListQuery : GetListQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
