namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalInvitationSyncCommandBase : BaseRequestType
    {
        public int BatchRenewalJobKey { get; set; }
        public string GUID { get; set; }
        public int InsuranceFileKey { get; set; }
        public int RecordsCount { get; set; }
    }
}
