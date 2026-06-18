namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUserDetailsQueryBase
    {
        public string LoginUserName { get; set; }
        public string UserName { get; set; }
        public int UserGroupsPageNumber { get; set; }
        public int UserGroupsPageSize { get; set; }
        public int SourceListsPageNumber { get; set; }
        public int SourceListPageSize { get; set; }
    }
}
