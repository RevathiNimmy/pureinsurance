using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPartyType
    {
        public BaseAddressType Address { get; set; }
        public string PartyClaimNumber { get; set; }
       // public BaseContactType[] Contact { get; set; }
        public List<BaseContactType> Contact { get; set; }
        public int PartyKey { get; set; }
        public string PartyEmail { get; set; }
        public string PartyFaxNo { get; set; }
        public string PartyMobileNo { get; set; }
        public string PartyTelNo { get; set; }
        public string PartyTelNoOff { get; set; }
    }
}
