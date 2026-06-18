namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListOfManualJournalTransactionDetails
{
    public class GetListOfManualJournalTransactionDetailsQuery : GetListOfManualJournalTransactionDetailsQueryBase //GetListOfManualJournalTransactionDetailsQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
