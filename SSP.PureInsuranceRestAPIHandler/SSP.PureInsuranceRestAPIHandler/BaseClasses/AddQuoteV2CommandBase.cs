namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddQuoteV2CommandBase : BaseAddQuoteV2RequestType
    {
        public string OldPolicyNumber { get; set; }
    }
}
