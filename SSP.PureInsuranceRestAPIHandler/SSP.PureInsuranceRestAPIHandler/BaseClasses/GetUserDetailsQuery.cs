namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserDetailsQuery : GetUserDetailsQueryBase
    {
        public string Route { get; set; }
        public string UserGroupsSortyBy { get; set; }
        public string SourceListSortBy { get; set; }
    }
}
