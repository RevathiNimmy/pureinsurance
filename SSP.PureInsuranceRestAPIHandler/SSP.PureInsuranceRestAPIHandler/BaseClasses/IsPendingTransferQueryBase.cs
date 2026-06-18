namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class IsPendingTransferQueryBase : BaseRequestType
    {   
        public int InsuranceFileKey { get; set;}
        public string PolicyNumber { get; set; } = string.Empty;
    }
}
