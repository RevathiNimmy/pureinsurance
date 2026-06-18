namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyAssociatesQueryBase : BaseRequestType
    {
        public int InsuranceFileKey { get; set; }
        public int InsuranceFolderCnt { get; set; }
    }
}
