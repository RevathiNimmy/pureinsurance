using SSP.PureInsuranceRestAPIHandler.Constants;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteIn
    {
        // In ONLY Parameters
        public string DataModelCode { get; set; } = "";
        public string BusinessTypeCode { get; set; } = "";
        public System.DateTime EffectiveDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string InsuredName { get; set; } = "";
        public int PartyCnt { get; set; } = 0;
        public int AgentCnt { get; set; } = 0;
        public string InsuranceFolderDescription { get; set; } = "";
        public string AlternateReference { get; set; } = "";
        public string BrokerABIID { get; set; } = "";
        public string RiskGroup { get; set; } = "";
        public string RiskCode { get; set; } = "";
        public int SourceId { get; set; }
        public int InsurerCnt { get; set; }
        public int ScreenId { get; set; }
        public int CurrencyId { get; set; }
        public int AnalysisId { get; set; }
        public string PolicyStatusCode { get; set; } = string.Empty;
        public int PolicyVersion { get; set; }
        public string LapsedReasonCode { get; set; } = "";
        public int LapsedReasonID { get; set; } = 0;
        public System.DateTime LapsedDate { get; set; } = InternalSamConstants.GISLowDate;
        public string LapsedReasonDescription { get; set; } = "";
        public System.DateTime InceptionDate { get; set; } = InternalSamConstants.GISLowDate;
        public System.DateTime InceptionDateTPI { get; set; } = InternalSamConstants.GISLowDate;
        public System.DateTime RenewalDate { get; set; } = InternalSamConstants.GISLowDate;
        public string OldPolicyNumber { get; set; } = "";
        public string AccountExecutiveShortname { get; set; } = "";
        public string AccountHandlerShortname { get; set; } = "";
        public string PolicyVersionTypeCode { get; set; } = "";
        public string sCoInsurancePlacement { get; set; } = string.Empty;

        // In/Outs
        public int InsuranceFolderCnt { get; set; }
        public int InsuranceFileCnt { get; set; }
        public string InsuranceFileRef { get; set; } = "";
        public AdditionalData[] AdditionalDataArray { get; set; }
        public int RiskCnt { get; set; } = 0;
        public int RiskGroupId { get; set; }
        public int RiskCodeId { get; set; }
    }

}
