namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseSettleAllClaimPaymentSummaryResponseType
    {
        public decimal Amount { get; set; }
        public string MediaTypeCode { get; set; }
        public int NoOfTransactions { get; set; }
        public string StatusOfTransaction { get; set; }
    }
}
