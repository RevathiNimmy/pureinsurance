namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindReinsurerQuery : FindReinsurerQueryBase
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
    }
}
