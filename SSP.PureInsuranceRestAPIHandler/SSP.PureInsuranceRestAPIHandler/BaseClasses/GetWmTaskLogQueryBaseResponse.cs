
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetWmTaskLogQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetWmTaskLogResponseTypeTaskLogRow> TaskLog { get; set; }
    }
}
