using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimResponseType : BaseResponseType
    {
        public int BaseClaimKey { get; set; }
        public int ClaimKey { get; set; }
        public string ClaimNumber { get; set; }
        public bool PaymentAuthorized { get; set; }
        public string ResultingStatus { get; set; }
        public byte[] ApiTimeStamp { get; set; }
        public int Version { get; set; }
        public System.Collections.Generic.List<BaseClaimResponseTypeWarnings> Warnings { get; set; }
    }
}
