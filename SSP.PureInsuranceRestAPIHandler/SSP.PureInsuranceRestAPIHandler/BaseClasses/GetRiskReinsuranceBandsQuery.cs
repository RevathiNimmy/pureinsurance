namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceBandsQuery : GetRiskReinsuranceBandsQueryBase//, IRequest<GetRatingDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
