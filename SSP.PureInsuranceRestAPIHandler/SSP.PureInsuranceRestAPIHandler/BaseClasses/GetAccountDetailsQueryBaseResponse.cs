namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountDetails
{
    public class GetAccountDetailsQueryBaseResponse : BaseResponseType
    {
        public double AccountBalance { get; set; }
        public string AccountName { get; set; }
        public string AccountStatus { get; set; }
        public string ContactName { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneExtension { get; set; }
        public string PhoneNumber { get; set; }
        public double TransactionCurrencyOSBalance { get; set; }
        public System.Collections.Generic.List<BaseGetAccountDetailsResponseTypeTransactions> Transactions { get; set; }
    }
}
