namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalAcceptCommandBase : BaseRequestType
    {
        public int BatchRenewalJobKey { get; set; }
        public string GUID { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public int RecordsCount { get; set; }
        public int SourceKey { get; set; }
    }
}
