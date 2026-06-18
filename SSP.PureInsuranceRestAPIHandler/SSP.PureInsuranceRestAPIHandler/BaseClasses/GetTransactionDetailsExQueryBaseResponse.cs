namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetailsEx
{
    public class GetTransactionDetailsExQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetTransactionDetailsExResponseTypeTransactions> Transactions { get; set; }
    }
}
