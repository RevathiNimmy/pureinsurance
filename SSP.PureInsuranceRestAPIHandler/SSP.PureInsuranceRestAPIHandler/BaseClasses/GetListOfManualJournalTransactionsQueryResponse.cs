namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListofManualJournalTransactions
{
    public class GetListOfManualJournalTransactionsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetListOfManualJournalTransactionsResponse> ManualJournalTransactions { get; set; }
    }
}
