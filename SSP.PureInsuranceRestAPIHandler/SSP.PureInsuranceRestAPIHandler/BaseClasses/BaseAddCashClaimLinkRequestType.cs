using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseAddCashClaimLinkRequestType :BaseRequestType
    {
        public int CashListItemKey{ get; set; }

        public int ClaimPaymentKey { get; set; }
    }
}
