using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetSharepointFileListQueryResponse : BasePagedResponse
    {
        public string FolderPath { get; set; }
        public List<BaseGetSharepointFileListResponseTypeItemList> ItemList { get; set; }
    }
}
