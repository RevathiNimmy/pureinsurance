namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyDetailsAdditionalDetailsType
    {
        public int PartyKey { get; set; }
        public string Name { get; set; }
        public int AccountCurrencyKey { get; set; }
        public bool IsDomiciledForTax { get; set; }
        public string TaxNumber { get; set; }
        public decimal TaxPercentage { get; set; }
        public bool IsTaxExempt { get; set; }
    }
}
