namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountBalance
{
    public class GetAccountBalanceQueryBaseResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetAccountBalanceResponseTypeRow> AccountBalance { get; set; }
    }
}
