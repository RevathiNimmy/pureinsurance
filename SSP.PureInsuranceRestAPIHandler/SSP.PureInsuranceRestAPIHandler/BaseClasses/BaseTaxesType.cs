namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseTaxesType
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TaxBandCode { get; set; }
        public int TaxBandID { get; set; }
        public decimal TaxRate { get; set; }
    }
}
