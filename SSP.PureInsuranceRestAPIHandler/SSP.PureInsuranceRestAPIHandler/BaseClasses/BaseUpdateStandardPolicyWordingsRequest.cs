using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateStandardPolicyWordingsRequest : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public System.Collections.Generic.List<BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow> PolicyStandardWordings { get; set; }
    }
}
