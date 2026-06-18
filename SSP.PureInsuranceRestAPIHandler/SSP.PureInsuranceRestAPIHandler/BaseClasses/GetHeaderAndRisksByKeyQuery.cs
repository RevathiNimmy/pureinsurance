namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndRisksByKeyQuery : GetHeaderAndRisksByKeyQueryBase// //GetHeaderAndRisksByKeyQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
