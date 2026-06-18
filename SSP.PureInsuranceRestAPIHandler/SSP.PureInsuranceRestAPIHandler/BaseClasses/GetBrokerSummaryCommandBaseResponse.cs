
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetBrokerSummaryCommandBaseResponse : BasePagedResponse
    {
        public List<BaseGetBrokerSummaryResponseTypeRow> InsuranceFileDetails { get; set; }
    }
}
