using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlansQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetFinancePlansResponseTypeRow> FinancePlans { get; set; }
    }
}
