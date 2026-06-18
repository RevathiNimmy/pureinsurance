using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPolicyAssociatesQueryResponse
    {
        public List<BaseGetPolicyAssociatesQueryBaseResponse> AssociatesQuery { get; set; }
    }
}
