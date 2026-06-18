
namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AddAddressModel
    {
        public int Row { get; set; }
        public object AddressKey { get; set; } = null;
        public object Address1 { get; set; } = null;
        public object Address2 { get; set; } = null;
        public object Address3 { get; set; } = null;
        public object Address4 { get; set; } = null;
        public object PostalCode { get; set; } = null;
        public object CountryID { get; set; } = null;
        public object ExternalId { get; set; } = null;
        public string Address5 { get; set; } = string.Empty;
        public string Address6 { get; set; } = string.Empty;
        public string Address7 { get; set; } = string.Empty;
        public string Address8 { get; set; } = string.Empty;
        public string Address9 { get; set; } = string.Empty;
        public string Address10 { get; set; } = string.Empty;
    }
}
