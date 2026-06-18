namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementLinesQuery : GetRiskReinsuranceArrangementLinesQueryBase
    {
		public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
