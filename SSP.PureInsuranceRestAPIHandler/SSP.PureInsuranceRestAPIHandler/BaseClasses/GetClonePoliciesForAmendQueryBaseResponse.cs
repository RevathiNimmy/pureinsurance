using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClonePoliciesForAmendQueryBaseResponse : BaseResponseType
    {
        public List<BaseGetClonePoliciesForAmendResponseTypePoliciesRow> Policies { get; set; }
    }
}
