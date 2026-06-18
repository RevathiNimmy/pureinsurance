using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetDocumentDefaultsQueryResponse : BasePagedResponse
    {
        public List<BaseGetDocumentDefaultsResponseTypeDocumentTemplates> DocumentTemplates { get; set; }
    }
}
