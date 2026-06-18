namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListofManualJournalTransactions
{
    public class GetListOfManualJournalTransactionsQueryBase : BaseRequestType
    {
        public string AccountCode { get; set; }
        public System.DateTime DateFrom { get; set; }
        public System.DateTime DateTo { get; set; }
        public string JournalTypeCode { get; set; }
        
        public string ManualJournalId { get; set; }
    }
}
