using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAccountDetailsResponseType : BaseResponseType
    {
        public string AccountName { get; set; }
        public string ContactName { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string AccountStatus { get; set; }
        public double AccountBalance { get; set; }
        public double TransactionCurrencyOSBalance { get; set; }
        public System.Collections.Generic.List<BaseGetAccountDetailsResponseTypeTransactions> Transactions { get; set; }
    }

}
