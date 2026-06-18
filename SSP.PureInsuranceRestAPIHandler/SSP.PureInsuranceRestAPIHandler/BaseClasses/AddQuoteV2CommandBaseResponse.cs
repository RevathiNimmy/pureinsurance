namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteV2CommandBaseResponse : BaseResponseType
    {
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public string InsuranceFileTypeCode { get; set; }
        public int InsuranceFolderKey { get; set; }
        public bool IsMandatoryRisk { get; set; }
        public bool IsMandatoryRiskSpecified { get; set; }
        public System.DateTime QuoteExpiryDate { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public int RiskFolderKey { get; set; }
        public int RiskKey { get; set; }
        public string XMLDataSet { get; set; }
    }
}
