namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetStandardPolicyWordingsQuery : GetStandardPolicyWordingsQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
