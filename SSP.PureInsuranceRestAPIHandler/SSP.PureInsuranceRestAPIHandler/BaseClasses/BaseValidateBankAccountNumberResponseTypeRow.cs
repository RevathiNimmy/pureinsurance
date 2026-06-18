namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseValidateBankAccountNumberResponseTypeRow
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string BankName { get; set; }
        public bool IsValid { get; set; }
        public bool IsValidSpecified { get; set; }
        public bool IsValidationOverridable { get; set; }
        public bool IsValidationOverridableSpecified { get; set; }
        public string PostalCode { get; set; }
        public string ValidationMessageDataset { get; set; }
    }
}
