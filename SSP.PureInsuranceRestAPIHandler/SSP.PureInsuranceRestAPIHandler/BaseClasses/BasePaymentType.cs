namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePaymentType
    {
        public int CashListItemKey { get; set; }
        public int CashListKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public string MediaReference { get; set; }
        public string MediaTypeCode { get; set; }
        public string OurReference { get; set; }
        public int PaymentAccountID { get; set; }
        public string PaymentTypeCode { get; set; }
        public string SubbranchCode { get; set; }
        public string TheirReference { get; set; }
        public int TransDetailKey { get; set; }
    }
}
