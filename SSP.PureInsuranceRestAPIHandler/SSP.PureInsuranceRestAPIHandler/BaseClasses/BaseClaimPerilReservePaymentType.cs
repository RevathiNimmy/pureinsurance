namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPerilReservePaymentType
    {
        public int BaseReserveKey { get; set; }
        public string TypeCode { get; set; }
        public decimal TotalReserve { get; set; }
        public decimal PaidToDate { get; set; }
        public decimal PaidToDateTax { get; set; }
        public decimal CurrentReserve { get; set; }
        public decimal ThisPaymentINCLTax { get; set; }
        public decimal ThisPaymentTax { get; set; }
        public decimal CostToClaim { get; set; }
    }

}
