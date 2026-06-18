using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRiskByProductQueryResponse : BasePagedResponse
    {
        public List<BaseGetRiskByProductResponseTypeRow> Risks { get; set; }
    }
}
