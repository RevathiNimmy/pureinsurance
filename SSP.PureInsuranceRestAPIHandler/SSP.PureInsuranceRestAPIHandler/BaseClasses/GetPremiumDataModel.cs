namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetPremiumDataModel
    {
        //[DBCol("RiskID")]
        public int RiskId { get; set; }

        //[DBCol("InsuranceFileCnt")]
        public int InsuranceFileCount { get; set; }

        //[DBCol("InsuranceFolderCnt")]
        public int InsuranceFolderCount { get; set; }

        public decimal PremiumDueGross { get; set; }

        public decimal PremiumDueNet { get; set; }

        public decimal PremiumDueTax { get; set; }

        public decimal TotalAnnualTax { get; set; }
    }
}
