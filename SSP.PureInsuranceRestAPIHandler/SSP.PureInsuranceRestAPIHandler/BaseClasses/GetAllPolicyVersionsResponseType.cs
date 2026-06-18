using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetAllPolicyVersionsResponseType : BaseResponseType
    {
        public List<BaseGetAllPolicyVersionsResponseTypeRow> Policies;
    }
}
