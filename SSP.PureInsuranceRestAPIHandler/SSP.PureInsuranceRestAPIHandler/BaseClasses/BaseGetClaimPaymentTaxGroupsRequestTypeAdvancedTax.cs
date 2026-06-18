namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimPaymentTaxGroupsRequestTypeAdvancedTax
    {
        public bool PayeeDomiciled { get; set; }
        public decimal PayeePercentage { get; set; }
        public string PayeeTaxNumber { get; set; }
    }
}
