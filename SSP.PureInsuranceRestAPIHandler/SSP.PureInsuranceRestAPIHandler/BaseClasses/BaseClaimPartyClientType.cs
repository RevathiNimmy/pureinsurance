namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPartyClientType : BaseClaimPartyType
    {
        public bool TaxRegistered { get; set; }
        public string TaxRegistrationNumber { get; set; } = string.Empty;
    }
}
