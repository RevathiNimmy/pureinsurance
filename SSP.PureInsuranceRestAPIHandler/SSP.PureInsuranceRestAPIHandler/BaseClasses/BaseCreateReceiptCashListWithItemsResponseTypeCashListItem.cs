namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreateReceiptCashListWithItemsResponseTypeCashListItem
    {
        public string AccountShortCode { get; set; }
        public int CashListItemKey { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentRef { get; set; }
        public int TransDetailKey { get; set; }
    }
}
