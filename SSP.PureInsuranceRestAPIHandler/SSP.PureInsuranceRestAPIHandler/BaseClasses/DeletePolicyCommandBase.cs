namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DeletePolicyCommandBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
    }
}
