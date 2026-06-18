namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListofManualJournalTransactions
{
    public class GetListOfManualJournalTransactionsQuery : GetListOfManualJournalTransactionsQueryBase //GetListOfManualJournalTransactionsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
