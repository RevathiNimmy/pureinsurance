using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class GetAllPolicyVersionsQueryResponse : BasePagedResponse
    {
        public List<BaseGetAllPolicyVersionsResponseTypeRow> Policies { get; set; }
    }
}
