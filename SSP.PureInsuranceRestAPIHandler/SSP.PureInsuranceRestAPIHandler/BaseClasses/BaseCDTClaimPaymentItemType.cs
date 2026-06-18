namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimPaymentItemType
    {
        public decimal PaymentAmount { get; set; }
        public string ReserveTypeCode { get; set; }
        public bool ReverseExcess { get; set; }
        public int SAMStagingClaimPaymentItemKey { get; set; }
        public int SiriusBaseReserveKey { get; set; }
        public string TaxGroupCode { get; set; }
    }
}
