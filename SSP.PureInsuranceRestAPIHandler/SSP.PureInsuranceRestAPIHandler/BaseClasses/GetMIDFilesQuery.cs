namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetMIDFilesQuery : GetMIDFilesQueryBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
