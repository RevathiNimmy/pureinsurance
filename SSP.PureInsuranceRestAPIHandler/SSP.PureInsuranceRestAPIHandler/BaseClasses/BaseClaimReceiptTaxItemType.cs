namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimReceiptTaxItemType
    {
        public string RecoveryType { get; set; }
        public string TaxGroupCode { get; set; }
        public string TaxBandCode { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public int ClassOfBusinessID { get; set; }
        public int IsManuallyChanges { get; set; }
        public int Sequence { get; set; }
        public int TaxBandId { get; set; }
        public int TaxGroupId { get; set; }
    }
}
