namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessPerilReserveType
    {
        public bool IsPaidToDate { get; set; }
        public bool IsReserveToDate { get; set; }
        public string TypeCode { get; set; }
        public decimal Amount { get; set; }
        public string TaxGroupCode { get; set; }
        public decimal PaymentAmount { get; set; }

        public bool PaymentAmountSpecified { get; set; }
        public bool ReverseExcess { get; set; } = false;
        public BaseClaimProcessPaymentDetailsType PaymentDetails { get; set; }
    }
}
