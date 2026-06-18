namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRenewalStatusQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }
}