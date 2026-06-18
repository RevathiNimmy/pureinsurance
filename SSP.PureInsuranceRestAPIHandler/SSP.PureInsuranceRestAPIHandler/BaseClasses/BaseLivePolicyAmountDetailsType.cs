namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseLivePolicyAmountDetailsType
    {
        public decimal AmountCollected { get; set; }
        public int InsuranceFileKey { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal PlanOutstandingAmount { get; set; }
        public decimal PolicyClientTaxesTotal { get; set; }
        public decimal PolicyFeesTotal { get; set; }
        public decimal PolicyNonClientTaxesTotal { get; set; }
        public decimal RiskClientTaxesTotal { get; set; }
        public decimal RiskFeesTotal { get; set; }
        public decimal RiskNonClientTaxesTotal { get; set; }
        public decimal ThisPremium { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
