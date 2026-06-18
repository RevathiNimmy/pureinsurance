using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindReceiptDetails
{
    public class FindReceiptDetailsQueryResponse : BasePagedResponse
    {
        public System.Collections.Generic.List<BaseFindReceiptDetailsResponseTypeReceiptDetailsRow> ReceiptDetails { get; set; }
    }
}
