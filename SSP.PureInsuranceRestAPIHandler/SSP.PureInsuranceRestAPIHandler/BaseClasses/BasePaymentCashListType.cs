using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePaymentCashListType : BaseCoreCashListType
    {
        public System.Collections.Generic.List<BasePaymentCashListItemType> PaymentItem { get; set; }

        public bool IsValid { get; set; }
    }
}
