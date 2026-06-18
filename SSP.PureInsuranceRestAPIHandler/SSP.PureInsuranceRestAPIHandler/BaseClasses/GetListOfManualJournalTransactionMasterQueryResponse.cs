namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetListOfManualJournalTransactionMaster
{
    public class GetListOfManualJournalTransactionMasterQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetListOfManualJournalTransactionMasterResponse> Masters { get; set; }
    }
}
