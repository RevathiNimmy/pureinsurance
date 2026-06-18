using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentListQueryResponse : BasePagedResponse
    {
        public List<BaseDocumentType> Documents { get; set; }
    }
}
