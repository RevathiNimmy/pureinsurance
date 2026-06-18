namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimReceiptAdvancedTaxDetailsType
    {
        public bool IsSettlement { get; set; }
        public bool IsSettlementSpecified { get; set; }
        public string PaymentToCode { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool IsTaxExemptSpecified { get; set; }
        public decimal ReceivableTaxPercentage { get; set; }
        public bool ReceivableTaxPercentageSpecified { get; set; }
        public bool InsuredDomiciled { get; set; }
        public bool InsuredDomiciledSpecified { get; set; }
        public decimal InsuredPercentage { get; set; }
        public bool InsuredPercentageSpecified { get; set; }
        public string InsuredTaxNumber { get; set; }
        public bool AdvancedTaxScriptOptionOn { get; set; }
        public string PayeeName { get; set; }
    }
}