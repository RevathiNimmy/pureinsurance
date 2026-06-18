namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public partial class BaseIsPendingTransferRequestType : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }

        public string PolicyNumber { get; set; } = string.Empty;

    }
}
