namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPaymentTaxItemType
    {
        public decimal Amount { get; set; }
        public int ClassOfBusinessID { get; set; }
        public int IsManuallyChanges { get; set; }
        public decimal Percentage { get; set; }
        public string ReserveType { get; set; }
        public int Sequence { get; set; }
        public string TaxBandCode { get; set; }
        public int TaxBandId { get; set; }
        public string TaxGroupCode { get; set; }
        public int TaxGroupId { get; set; }
    }
}
