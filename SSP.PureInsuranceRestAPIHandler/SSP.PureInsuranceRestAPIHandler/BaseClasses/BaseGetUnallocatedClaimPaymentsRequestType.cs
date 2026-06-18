namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetUnallocatedClaimPaymentsRequestType : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public bool PaymentDateSpecified { get; set; }
        public string ShortCode { get; set; }
        public bool ShortCodeSpecified { get; set; }
        public System.DateTime PaymentDateTo { get; set; }
        public bool PaymentDateToSpecified { get; set; }
    }

}
