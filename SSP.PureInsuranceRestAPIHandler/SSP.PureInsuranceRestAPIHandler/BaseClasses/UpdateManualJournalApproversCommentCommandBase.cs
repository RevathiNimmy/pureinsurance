namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.UpdateManualJournalApproversComment
{
    public class UpdateManualJournalApproversCommentCommandBase : BaseRequestType
    {
        public int ManualJournalId { get; set; }
        public string Comment { get; set; }
    }
}
