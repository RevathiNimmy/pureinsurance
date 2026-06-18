using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreatePaymentCashListWithItemsResponseType : BaseResponseType
    {
        public System.Collections.Generic.List<BaseCreatePaymentCashListWithItemsResponseTypeCashListItem> CashListItem { get; set; }
        public int CashListKey { get; set; }

        public BaseCreatePaymentCashListWithItemsResponseTypeCashListItem[] ArrCashListItem { get; set; } = new BaseCreatePaymentCashListWithItemsResponseTypeCashListItem[0];
    }
}