using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindDMEDocumentsQueryResponse : BasePagedResponse
    {
        public List<BaseDMEFolderType> Folders { get; set; }
        public List<BaseDocumentType> Documents { get; set; }
        public object DocumentsFirstPage { get; set; }
        public object DocumentsLastPage { get; set; }
        public object DocumentsPreviousPage { get; set; }
        public object DocumentsNextPage { get; set; }
        public object DocumentsPageNumber { get; set; }
        public object DocumentsPageSize { get; set; }
        public object DocumentsTotalPages { get; set; }
        public object DocumentsTotalRecords { get; set; }
        public object FoldersFirstPage { get; set; }
        public object FoldersLastPage { get; set; }
        public object FoldersPreviousPage { get; set; }
        public object FoldersNextPage { get; set; }
        public object FoldersPageNumber { get; set; }
        public object FoldersPageSize { get; set; }
        public object FoldersTotalPages { get; set; }
        public object FoldersTotalRecords { get; set; }
    }
}
