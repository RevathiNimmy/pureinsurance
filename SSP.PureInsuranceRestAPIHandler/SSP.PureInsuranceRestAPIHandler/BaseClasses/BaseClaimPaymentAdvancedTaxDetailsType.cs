using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{

    public class BaseClaimPaymentAdvancedTaxDetailsType
    {
        public bool InsuredDomiciled { get; set; }
        public decimal InsuredPercentage { get; set; }
        public string InsuranceTaxNumber { get; set; }
        public bool PayeeDomiciled { get; set; }
        public decimal PayeePercentage { get; set; }
        public string PayeeTaxNumber { get; set; }
        public string SafeHarbourCode { get; set; }
        public Nullable<int> SafeHarbourID { get; set; }
        public Nullable<decimal> SafeHarbourPercentage { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool IsWHTExempt { get; set; }
        public bool IsSettlement { get; set; }
        public string PaymentToCode { get; set; }
        public string PayeeName { get; set; }
        public bool IsExcess { get; set; }
        public bool AdvancedTaxScriptOptionOn { get; set; }
        public string PaymentTo { get; set; }
    }

}