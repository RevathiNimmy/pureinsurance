using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreateReceiptCashListWithItemsResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseCreateReceiptCashListWithItemsResponseTypeCashListItem> CashListItem { get; set; }
        public int CashListKey { get; set; }
    }
}
