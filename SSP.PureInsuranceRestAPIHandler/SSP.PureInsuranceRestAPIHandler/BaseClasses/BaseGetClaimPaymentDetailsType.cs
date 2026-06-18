using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentDetailsType
    {
        public bool IsThisPayment { get; set; }
        public string UltimatePayee { get; set; }
        public string OurRef { get; set; }
        public int ClaimKey { get; set; }
        public string PartyPaidCode { get; set; }
        public int ClaimPerilId { get; set; }
        public string CurrencyDescription { get; set; }
        public string PartyPaidName { get; set; }
        public decimal LossAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public int BaseClaimPaymentKey { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public string CurrencyCode { get; set; }
        public int PartyKey { get; set; }
        public string PaymentPartyType { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsReferred { get; set; }
        public BaseClaimPaymentAdvancedTaxDetailsType AdvancedTaxDetails { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public System.Collections.Generic.List<BaseGetClaimPaymentItemDetailsType> ClaimPaymentItems { get; set; }
        public bool IsExGratia { get; set; }
        public string LossCurrencyCode { get; set; }
    }

}