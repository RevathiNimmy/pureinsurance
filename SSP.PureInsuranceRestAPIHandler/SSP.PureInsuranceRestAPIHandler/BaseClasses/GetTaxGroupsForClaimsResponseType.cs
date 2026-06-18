using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetTaxGroupsForClaimsResponseType : BaseResponseType
    {
        public List<BaseGetTaxGroupsForClaimsResponseTypeRow> taxGroups;
    }
}
