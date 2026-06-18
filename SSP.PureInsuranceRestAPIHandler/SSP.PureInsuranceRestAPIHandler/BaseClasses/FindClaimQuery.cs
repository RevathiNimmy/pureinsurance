namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindClaimQuery : FindClaimQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; } = string.Empty;
    }
}
