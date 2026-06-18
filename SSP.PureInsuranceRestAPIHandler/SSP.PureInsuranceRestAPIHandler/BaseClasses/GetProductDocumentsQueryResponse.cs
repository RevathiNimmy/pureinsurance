
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductDocumentsQueryResponse : BasePagedResponse
    {
        public GetProductDocumentsQueryBaseResponse GetProductDocumentsResponse { get; set; }
    }
}
