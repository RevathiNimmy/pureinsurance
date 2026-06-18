namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalAcceptSyncCommandBase : BaseRequestType
    {
        public int BatchRenewalJobKey { get; set; }
        public string GUID { get; set; }

        public int InsuranceFileKey { get; set; }
        public int RecordsCount { get; set; }
    }
}
