namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseUpdateTaxesRequestTypeRow
    {
        public bool IsEdited { get; set; }
        public bool IsValue { get; set; }

        ////(1, int.MaxValue, ErrorMessage = "The TaxCalculationKey field is required")]
        //
        public int TaxCalculationKey { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxValue { get; set; }
    }
}
