namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAddressRequestType : BaseRequestType
    {
        public int AddressKey { get; set; }
        public int PartyKey { get; set; }
        public bool PartyKeySpecified { get; set; }
    }
}
