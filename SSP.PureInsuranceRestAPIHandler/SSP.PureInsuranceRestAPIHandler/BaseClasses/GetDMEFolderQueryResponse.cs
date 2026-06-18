using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDMEFolderQueryResponse
    {
        public List<BaseDocumentType> Documents { get; set; }
        public int ParentNum { get; set; }
        public List<BaseDMEFolderType> SubFolders { get; set; }
        public BasePagedResponse PagedDocuments { get; set; }
        public BasePagedResponse PagedSubFolders { get; set; }
    }
}
