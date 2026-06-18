namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteOut
    {
        public int InsuranceFolderCnt { get; set; } = 0;
        public int InsuranceFileCnt { get; set; } = 0;
        public string InsuranceFileRef { get; set; } = "";

        public int InsurerCnt { get; set; } = 0;
        public int AgentCnt { get; set; } = 0;
        public AdditionalData[] AdditionalDataArray { get; set; }
        public int RiskCnt { get; set; } = 0;
        public int RiskGroupId { get; set; }
        public int RiskCodeId { get; set; }
        public int GISSchemeID { get; set; } = 0;
    }

}
