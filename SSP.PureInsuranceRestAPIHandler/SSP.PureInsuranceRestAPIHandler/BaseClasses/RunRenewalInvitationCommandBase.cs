namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalInvitationCommandBase : BaseRequestType
    {
        public int BatchRenewalJobKey { get; set; }
        public string GUID { get; set; }
        public int InsuranceFileKey { get; set; }
        public int RecordsCount { get; set; }
        public int SourceKey { get; set; }
    }
}
