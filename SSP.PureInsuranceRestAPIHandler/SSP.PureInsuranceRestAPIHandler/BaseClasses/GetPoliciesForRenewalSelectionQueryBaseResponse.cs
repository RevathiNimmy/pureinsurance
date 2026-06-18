using System.Collections.Generic;
using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesForRenewalSelectionQueryBaseResponse : BasePagedResponse
    {

        public List<BaseGetPoliciesForRenewalSelectionResponseTypeRow> Policies { get; set; }


    }
}
