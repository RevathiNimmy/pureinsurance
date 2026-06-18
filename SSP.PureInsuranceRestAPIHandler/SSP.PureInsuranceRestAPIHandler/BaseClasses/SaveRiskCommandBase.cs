namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class SaveRiskCommandBase : BaseRequestType
    {

        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public byte[] QuoteTimeStamp { get; set; } = new byte[0];
        public int RiskKey { get; set; }
        public string XMLDataSet { get; set; } = string.Empty;
        public bool RemoveAllSW { get; set; }
    }
}
