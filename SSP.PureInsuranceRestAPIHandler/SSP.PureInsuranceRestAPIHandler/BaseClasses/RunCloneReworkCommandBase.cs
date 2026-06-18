namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunCloneReworkCommandBase : BaseRequestType
    {
        public int ClaimKey { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileType { get; set; }
        public bool IsFailed { get; set; }
        public int RiskKey { get; set; }
    }
}
