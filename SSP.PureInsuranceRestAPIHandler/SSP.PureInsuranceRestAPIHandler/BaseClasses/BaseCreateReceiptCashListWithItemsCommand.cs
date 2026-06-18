namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreateReceiptCashListWithItemsCommand : BaseRequestType
    {
        public int CashListKey { get; set; }

        public BaseReceiptCashListType ReceiptCashList { get; set; }
        public int SourceId { get; set; }
    }
}
