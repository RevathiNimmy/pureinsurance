namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetStandardPolicyWordingsQueryBase : BaseRequestType
    {
        public bool GetFreshPolicyStandardWording { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
