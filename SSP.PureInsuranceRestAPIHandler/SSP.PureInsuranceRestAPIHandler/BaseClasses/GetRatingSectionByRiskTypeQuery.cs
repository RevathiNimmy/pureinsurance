namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingSectionByRiskTypeQuery : GetRatingSectionByRiskTypeQueryBase//, IRequest<GetRatingDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
