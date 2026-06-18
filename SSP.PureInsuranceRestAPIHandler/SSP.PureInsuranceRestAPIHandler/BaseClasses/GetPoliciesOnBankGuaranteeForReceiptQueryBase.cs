using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPoliciesOnBankGuaranteeForReceiptQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }

        public int PartyKey { get; set; }

        public BGGetPoliciesActionType GetPoliciesFor { get; set; }
    }
}