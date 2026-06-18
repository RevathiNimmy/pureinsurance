namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetReceiptCashListItemsResponseTypeRow
    {
        public string AccountShortCode { get; set; }
        public double Amount { get; set; }
        public int CashListItemKey { get; set; }
        public bool Letter { get; set; }
        public string MediaReference { get; set; }
        public string MediaType { get; set; }
        public string Status { get; set; }
    }
}
