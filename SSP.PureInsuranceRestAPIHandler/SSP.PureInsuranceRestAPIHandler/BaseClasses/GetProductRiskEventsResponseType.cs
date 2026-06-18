using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductRiskEventsResponseType : BaseResponseType
    {
        public List<BaseGetProductRiskEventsResponseTypeRow> Events { get ;set; }
    }
}
