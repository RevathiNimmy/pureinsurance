using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateReceiptCashListItemCommandBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseCreateReceiptCashListItemResponseTypeCashListItem> CashListItem { get; set; }
    }
}
