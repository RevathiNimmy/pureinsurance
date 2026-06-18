namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindInsuranceFileForClaimsCommand : FindInsuranceFileForClaimsCommandBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; } = null;
    }
}
