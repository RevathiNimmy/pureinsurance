namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskResponseType
    {
        public int RiskKey { get; set; }
        public string XMLDataSet { get; set; }
        public decimal PremiumDueNet { get; set; }
        public decimal PremiumDueTax { get; set; }
        public decimal PremiumDueGross { get; set; }
        public decimal TotalAnnualTax { get; set; }
        public decimal CommissionAmount { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public decimal PolicyLevelTax { get; set; }
        public decimal PolicyLevelFees { get; set; }
        public decimal ProRataRate { get; set; }
        public BaseTaxesAndFeesType PolicyLevelTaxesAndFees { get; set; }
        public BaseTaxesAndFeesType RiskLevelTaxesAndFees { get; set; }
    }
}
