using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreateReceiptCashListItemResponseTypeCashListItem
    {
        public string AccountShortCode { get; set; }
        public int CashListItemKey { get; set; }
        public int TransDetailKey { get; set; }
        public System.Collections.Generic.List<string> AllocationStatus { get; set; }
        public System.Collections.Generic.List<int> InsuranceFileKey { get; set; }
    }
}
