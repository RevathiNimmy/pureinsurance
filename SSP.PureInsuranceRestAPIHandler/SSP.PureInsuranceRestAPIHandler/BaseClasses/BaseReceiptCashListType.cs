using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseReceiptCashListType : BaseCoreCashListType
    {
        public System.Collections.Generic.List<BaseReceiptCashListItemType> ReceiptItem { get; set; }
    }
}
