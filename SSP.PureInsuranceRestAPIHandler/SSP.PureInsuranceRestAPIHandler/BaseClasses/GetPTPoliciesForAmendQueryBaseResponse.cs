using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPTPoliciesForAmendQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetPTPoliciesForAmendResponseTypePoliciesRow> Policies { get; set; }
    }
}
