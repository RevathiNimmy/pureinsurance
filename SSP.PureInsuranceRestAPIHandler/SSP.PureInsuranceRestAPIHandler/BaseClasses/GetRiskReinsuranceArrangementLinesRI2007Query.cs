namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskReinsuranceArrangementLinesRI2007Query: GetRiskReinsuranceArrangementLinesRI2007QueryBase
    {
		public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
