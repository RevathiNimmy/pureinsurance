using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindPaymentDetails
{
    public class FindPaymentDetailsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseFindPaymentDetailsResponseTypePaymentDetails> PaymentDetails { get; set; }
    }
}
