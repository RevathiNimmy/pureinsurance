namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductsForUserBranchQuery : BaseRequestType
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public int AgentKey { get; set; }
    }
}
