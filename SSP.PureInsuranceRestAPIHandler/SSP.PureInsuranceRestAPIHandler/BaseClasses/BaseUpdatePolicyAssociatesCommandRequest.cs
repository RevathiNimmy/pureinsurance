using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdatePolicyAssociatesCommandRequest : BaseRequestType
    {
        public System.Collections.Generic.List<BasePolicyAssociatesType> Associates { get; set; }
        public bool SkipPolicyTypeCheck { get; set; }
        public byte[] ApiTimeStamp { get; set; }
    }
}
