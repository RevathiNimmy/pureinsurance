namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListOfManualJournalTransactionMaster
{
    public class GetListOfManualJournalTransactionMasterQuery : GetListOfManualJournalTransactionMasterQueryBase //GetListOfManualJournalTransactionMasterQueryResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
    }
}
