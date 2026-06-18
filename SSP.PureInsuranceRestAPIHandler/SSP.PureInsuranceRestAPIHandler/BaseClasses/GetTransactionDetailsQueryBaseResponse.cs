namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetTransactionDetails
{
    public class GetTransactionDetailsQueryBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseGetTransactionDetailsResponseTypeRow> Transactions { get; set; }
    }
}
