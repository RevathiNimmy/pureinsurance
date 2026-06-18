namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreatePaymentCashListWithItemsResponseTypeCashListItem
    {
        public int CashListItemKey { get; set; }
        public int TransDetailKey { get; set; }
        public bool AutoAllocatePaymentSuccessful { get; set; }
        public string AccountShortCode { get; set; }
        public string DocumentRef { get; set; }
        public string DocumentCode { get; set; }
    }

}
