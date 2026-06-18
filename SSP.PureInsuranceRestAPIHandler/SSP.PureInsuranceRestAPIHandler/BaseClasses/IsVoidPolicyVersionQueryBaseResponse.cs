namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class IsVoidPolicyVersionQueryBaseResponse : BaseResponseType
    {
        public bool IsValidVoidPolicyVersion { get; set; }
        public bool IsInstalmentExists { get; set; }
        public bool IsQuoteExists { get; set; }
    }
}
