namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAgentSettingsQuery : GetAgentSettingsQueryBase
    {
        public int BranchesPageNumber { get; set; }
        public int BranchesPageSize { get; set; }
        public int ContactsPageNumber { get; set; }
        public int ContactsPageSize { get; set; }
        public int UsersPageNumber { get; set; }
        public int UsersPageSize { get; set; }
        public string BranchesSortBy { get; set; }
        public string ContactsSortBy { get; set; }
        public string UsersSortBy { get; set; }
    }
}
