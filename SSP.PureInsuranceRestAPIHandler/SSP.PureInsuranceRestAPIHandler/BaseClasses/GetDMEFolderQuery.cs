namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDMEFolderQuery : GetDMEFolderQueryBase
    {
        public int DocumentsPageNumber { get; set; }
        public int DocumentsPageSize { get; set; }
        public string DocumentsSortBy { get; set; }
        public int SubFoldersPageNumber { get; set; }
        public int SubFoldersPageSize { get; set; }
        public string SubFoldersSortBy { get; set; }
    }
}
