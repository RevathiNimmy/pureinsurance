using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimPaymentTaxGroupsQueryBase : BaseRequestType
    {
        public BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax AdvancedTax { get; set; }
        public int PartyKey { get; set; }
        public ClaimPaymentPartyTypeType PaymentPartyType { get; set; }
    }
}
