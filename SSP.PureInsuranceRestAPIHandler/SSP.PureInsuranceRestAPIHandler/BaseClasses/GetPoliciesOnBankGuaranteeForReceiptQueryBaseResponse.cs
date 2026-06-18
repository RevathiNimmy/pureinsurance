using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesOnBankGuaranteeForReceiptQueryBaseResponse : BaseResponseType
    {
        public string PartyCode { get; set; }
        public string ResolvedName { get; set; }

        public List<BaseGetPoliciesOnBankGuaranteeForReceiptResponseType>? GetPoliciesResponse { get; set; }
    }
}