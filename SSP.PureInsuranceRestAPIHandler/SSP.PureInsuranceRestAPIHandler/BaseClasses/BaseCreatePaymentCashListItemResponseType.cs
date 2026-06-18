using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreatePaymentCashListItemResponseType : BaseResponseType
    {
        public System.Collections.Generic.List<int> CashListItemKey { get; set; }
        public System.Collections.Generic.List<int> TransDetailKey { get; set; }
    }
}
