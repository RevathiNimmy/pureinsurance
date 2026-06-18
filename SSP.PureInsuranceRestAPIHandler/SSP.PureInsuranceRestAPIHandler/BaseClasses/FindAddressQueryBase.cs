namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindAddressQueryBase : BaseRequestType
    {
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine10 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string AddressLine3 { get; set; } = string.Empty;
        public string AddressLine4 { get; set; } = string.Empty;
        public string AddressLine5 { get; set; } = string.Empty;
        public string AddressLine6 { get; set; } = string.Empty;
        public string AddressLine7 { get; set; } = string.Empty;
        public string AddressLine8 { get; set; } = string.Empty;
        public string AddressLine9 { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int CountryCodeId { get; set; }
        public string PostCode { get; set; } = string.Empty;
    }
}
