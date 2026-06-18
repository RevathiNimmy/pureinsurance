using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDMEFolderQueryBaseResponse : BaseResponseType
    {
        public List<BaseDocumentType> Documents { get; set; }
        public int ParentNum { get; set; }
        public List<BaseDMEFolderType> SubFolders { get; set; }
    }
}
