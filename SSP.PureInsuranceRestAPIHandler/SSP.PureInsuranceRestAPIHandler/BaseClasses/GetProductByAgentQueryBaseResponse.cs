using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductByAgentQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetProductByAgentResponseTypeRow> Products { get; set; }
    }
}
