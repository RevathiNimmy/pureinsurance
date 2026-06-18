namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateVoidPolicyVersionCommandBaseResponse : BaseResponseType
    {
        public int NewInsuranceFileKey { get; set; }
        public string? FailureMessage { get; set; }
    }
}