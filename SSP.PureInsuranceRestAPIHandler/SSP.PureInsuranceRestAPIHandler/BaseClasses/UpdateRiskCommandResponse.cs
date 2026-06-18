namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateRiskCommandResponse : BaseResponseType
    {
        public decimal PremiumDueNet { get; set; }
        public decimal PremiumDueTax { get; set; }
        public decimal PremiumDueGross { get; set; }
        public decimal TotalAnnualTax { get; set; }
        public decimal CommissionAmount { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public decimal PolicyLevelTax { get; set; }
        public decimal PolicyLevelFees { get; set; }
        public BaseTaxesAndFeesType PolicyLevelTaxesAndFees { get; set; }
        public BaseTaxesAndFeesType RiskLevelTaxesAndFees { get; set; }
        public int ProRata { get; set; }

        public bool ProRataSpecified { get; set; }
        public decimal ProRataRate { get; set; }

        public bool ProRataRateSpecified { get; set; }
        public string ProRataMessage { get; set; }
        public bool ReturnPremiumMoreThanBilled { get; set; }
        public string XMLDataSet { get; set; }
    }
}
