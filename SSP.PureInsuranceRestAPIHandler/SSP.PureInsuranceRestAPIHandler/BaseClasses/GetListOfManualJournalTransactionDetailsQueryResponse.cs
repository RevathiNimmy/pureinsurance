namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListOfManualJournalTransactionDetails
{
    public class GetListOfManualJournalTransactionDetailsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetListOfManualJournalTransactionDetailsResponse> ManualJournalTransactionDetails { get; set; }
    }
}
