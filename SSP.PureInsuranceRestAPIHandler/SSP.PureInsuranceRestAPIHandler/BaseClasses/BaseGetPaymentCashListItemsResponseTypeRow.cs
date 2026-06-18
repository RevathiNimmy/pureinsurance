namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPaymentCashListItemsResponseTypeRow
    {
        //[DBCol("AccountShortCode")]
        public string AccountShortCode { get; set; }
        //[DBCol("Amount")]
        public double Amount { get; set; }
        //[DBCol("CashListItemKey")]
        public int CashListItemKey { get; set; }
        //[DBCol("Letter")]
        public bool Letter { get; set; }
        //[DBCol("MediaReference")]
        public string MediaReference { get; set; }
        //[DBCol("MediaType")]
        public string MediaType { get; set; }
        //[DBCol("Status")]
        public string Status { get; set; }
    }
}
