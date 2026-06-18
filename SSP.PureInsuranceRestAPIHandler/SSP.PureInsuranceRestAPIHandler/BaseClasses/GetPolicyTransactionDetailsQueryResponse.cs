namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetPolicyTransactionDetails
{
    public class GetPolicyTransactionDetailsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseGetPolicyTransactionDetailsResponseType> Transactions { get; set; }
    }
}
