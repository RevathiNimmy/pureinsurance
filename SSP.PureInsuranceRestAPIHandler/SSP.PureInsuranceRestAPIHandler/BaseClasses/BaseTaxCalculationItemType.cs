namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTaxCalculationItemType
    {
        public int TaxCalculationItemId { get; set; }
        public string TaxBandCode { get; set; }
        public int TaxGroupId { get; set; }
        public int TaxBandId { get; set; }
        public string TaxCurrencyCode { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxValue { get; set; }
        public bool IsValue { get; set; }
        public int ClassOfBusinessId { get; set; }
        public int Sequence { get; set; }
        public bool IsManuallyChanged { get; set; }
        public string TaxGroupDescription { get; set; }
        public string TaxBandDescription { get; set; }
        public int CurrencyId { get; set; }
        public string TaxScriptName { get; set; }
        public string RecoveryType { get; set; }
        public string TaxGroupCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
