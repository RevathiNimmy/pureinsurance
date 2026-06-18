using SSP.PureInsuranceRestAPIHandler.BaseClasses;

namespace SSP.PureInsuranceRestAPIHandler
{
    public class GetPartyQueryResponse : BasePagedResponse
    {
        public BasePartyPCType PCType { get; set; }
        public BasePartyCCType CCType { get; set; }
        public BasePartyOtherType OTHERType { get; set; }
        public int NoofClosedClaims { get; set; }
        public int NoofOpenClaims { get; set; }
        public int NoofPolicies { get; set; }
        public byte[] PartyTimestamp { get; set; } = new byte[0];
    }
}
