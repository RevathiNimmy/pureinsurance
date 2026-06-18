using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlansQueryResponse : BasePagedResponse
    {
        public GetFinancePlansQueryBaseResponse GetFinancePlansResponse { get; set; }

    }
}
