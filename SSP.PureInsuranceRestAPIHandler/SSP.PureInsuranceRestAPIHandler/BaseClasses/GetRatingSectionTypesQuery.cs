namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRatingSectionTypesQuery : GetRatingSectionTypesQueryBase//, IRequest<GetRatingDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
