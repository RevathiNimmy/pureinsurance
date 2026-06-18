namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindUsersQueryBase : BaseRequestType
    {
        public string FullName { get; set; } = string.Empty;
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int AgentKey { get; set; }
    }
}
