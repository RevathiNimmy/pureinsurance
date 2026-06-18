namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePaymentCashListItemTypePolicies
    {
        public decimal AmountTobeAllocated { get; set; }
        public string DocumentRef { get; set; }
        public int InsuranceFileKey { get; set; }
        public bool IsCurrencyWriteOff { get; set; }
        public decimal WriteOffAmount { get; set; }
        public int WriteOffReasonKey { get; set; }
    }
}
