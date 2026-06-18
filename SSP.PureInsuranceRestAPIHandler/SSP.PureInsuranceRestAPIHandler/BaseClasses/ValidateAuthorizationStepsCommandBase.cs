namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ValidateAuthorizationSteps
{
    public class ValidateAuthorizationStepsCommandBase : BaseRequestType
    {
        public int ManualJournalId { get; set; }
        public bool IsApproved { get; set; }
    }
}
