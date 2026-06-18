namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndRisksByKeyResponseTypeRow
    {
        public BaseCoverNoteRiskItemType CoverNote { get; set; }
        public string Coverage { get; set; }
        public string Description { get; set; }
        public bool Discounted { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Extensions { get; set; }
        public double FeePremium { get; set; }
        public double FeeTax { get; set; }
        public string InsuredItem { get; set; }
        public bool IsMandatoryRisk { get; set; }
        public bool IsRisk { get; set; }
        public bool Is_Auto_Rated { get; set; }
        public int OriginalRiskKey { get; set; }
        public double Premium { get; set; }
        public int RiskFolderKey { get; set; }
        public int RiskKey { get; set; }
        public int RiskNumber { get; set; }
        public double RiskTax { get; set; }
        public string RiskTypeCode { get; set; }
        public string RiskTypeDescription { get; set; }
        public double StampDutyInsured { get; set; }
        public double StampDutyInsurer { get; set; }
        public System.DateTime StartDate { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public double TotalSumInsured { get; set; }
        public int Variation { get; set; }
        public System.DateTime riskLinkChangeDate { get; set; }
        public string riskLinkStatusFlag { get; set; }
    }
}
