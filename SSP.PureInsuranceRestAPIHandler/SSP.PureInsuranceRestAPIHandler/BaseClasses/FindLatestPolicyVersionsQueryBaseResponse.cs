using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindLatestPolicyVersionsQueryBaseResponse: BasePagedResponse
    {
        public List<BaseFindLatestPolicyVersionsResponseTypeRow> InsuranceFileDetails { get; set; }
    }
}
