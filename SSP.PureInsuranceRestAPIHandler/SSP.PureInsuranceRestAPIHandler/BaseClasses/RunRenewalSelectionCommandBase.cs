namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalSelectionCommandBase : BaseRequestType
    {
        public int BatchRenewalJobKey { get; set; }
        public string GUID { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public int RecordsCount { get; set; }
        public bool DoNotCreateTMPAnniversaryVersion { get; set; }
    }
}
