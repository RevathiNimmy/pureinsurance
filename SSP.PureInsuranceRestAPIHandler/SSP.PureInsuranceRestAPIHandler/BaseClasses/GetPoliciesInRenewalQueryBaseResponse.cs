using System.Collections.Generic;
using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesInRenewalQueryBaseResponse : BasePagedResponse
    {
       
        public List<BaseGetPoliciesInRenewalResponseTypeRow> Policies { get; set; }
        

    }
}
