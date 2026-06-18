namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInstalmentQuotesQuery : GetInstalmentQuotesQueryBase//, IRequest<GetHeaderAndPolicyFeesByKeyQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
