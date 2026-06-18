namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateVoidPolicyVersionCommandBase : BaseRequestType
    {
       public int InsuranceFileKey { get; set; }        
        public int InsuranceFolderKey { get; set; }
    }
}