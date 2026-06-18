namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetEventNoteQuery : GetEventNoteQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
