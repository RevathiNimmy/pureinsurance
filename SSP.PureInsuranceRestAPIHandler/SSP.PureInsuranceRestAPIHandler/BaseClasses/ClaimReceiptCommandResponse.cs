using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ClaimReceiptCommandResponse : BaseResponseType
    {
        public int ClaimKey { get; set; }
        public int BaseClaimKey { get; set; }
        public string ClaimNumber { get; set; }
        public int Version { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public string ResultingStatus { get; set; }
        public List<BaseClaimResponseTypeWarnings> Warnings { get; set; }
        public bool PaymentAuthorized { get; set; }
    }
}
