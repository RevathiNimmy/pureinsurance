namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CheckInsuranceFile
    {
        public int InsuranceFolderKey { get; set; }
        public int SourceId { get; set; }
        public bool IsCheckInsuranceFile { get; set; }
        public int LeadAgentKey { get; set; }
        public int InsuredKey { get; set; }
        public string InsuranceRef { get; set; } = string.Empty;
    }
}
