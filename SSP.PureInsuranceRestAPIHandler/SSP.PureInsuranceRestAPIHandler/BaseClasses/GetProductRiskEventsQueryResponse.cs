
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductRiskEventsQueryResponse : BasePagedResponse
    {
        public List<BaseGetProductRiskEventsResponseTypeRow> Events { get; set; }
    }
}