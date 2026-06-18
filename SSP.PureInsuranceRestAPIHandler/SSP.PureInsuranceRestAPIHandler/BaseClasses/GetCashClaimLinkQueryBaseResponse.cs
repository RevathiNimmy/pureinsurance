namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCashClaimLinkQueryBaseResponse : BaseResponseType
    {
        public decimal Amount { get; set; }
        public string BranchCode { get; set; }
        public int CashListItemKey { get; set; }
        public int CashListKey { get; set; }
        public string CurrencyCode { get; set; }
        public string MediaTypeCode { get; set; }
    }
}
