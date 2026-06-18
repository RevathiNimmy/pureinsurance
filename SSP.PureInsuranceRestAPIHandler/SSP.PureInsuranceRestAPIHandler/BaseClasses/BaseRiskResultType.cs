namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskResultType
    {
        public int SAMStagingRiskKey { get; set; }
        public int RiskFolderID { get; set; }
        public int RiskID { get; set; }
        public string XMLDataSet { get; set; }
        public decimal PremiumDueNet { get; set; }
        public decimal PremiumDueTax { get; set; }
        public decimal PremiumDueGross { get; set; }
        public decimal TotalAnnualTax { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal PolicyLevelTax { get; set; }
        public STSErrorType STSError { get; set; }
    }
}
