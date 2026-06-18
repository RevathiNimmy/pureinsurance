using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyAssociatesQueryBaseResponse : BaseResponseType
    {
        public BaseAddressType Address { get; set; }
        public BasePolicyAssociatesType Associates { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public PartyTypeType PartyType { get; set; }
    }
}
