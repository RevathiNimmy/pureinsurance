using Newtonsoft.Json;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CreateReceiptCashListItemCommandBase : BaseRequestType
    {
        public int CashListKey { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int SourceId { get; set; }
        public System.Collections.Generic.List<BaseReceiptCashListItemType> ReceiptCashListItem { get; set; }
    }
}
