namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalSelectionByPolicyCommandBase : BaseRequestType
    {

        ////(1, int.MaxValue, ErrorMessage = "The InsuranceFileKey field is required")]
        //
        public int InsuranceFileKey { get; set; }
        public bool SetRenewalInviteAsSent { get; set; }

        public int SourceId { get; set; }

        public bool DoNotCopyRiskAtRenSelection { get; set; }

        public string Regarding { get; set; }

        public bool SkipGenerateRenewalPolicyNumber { get; set; }
    }
}
