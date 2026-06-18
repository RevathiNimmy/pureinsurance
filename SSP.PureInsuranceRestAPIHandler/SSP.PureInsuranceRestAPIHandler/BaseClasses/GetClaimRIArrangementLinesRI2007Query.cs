namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRIArrangementLinesRI2007Query : BaseGetClaimRIArrangementLinesRI2007RequestType
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
