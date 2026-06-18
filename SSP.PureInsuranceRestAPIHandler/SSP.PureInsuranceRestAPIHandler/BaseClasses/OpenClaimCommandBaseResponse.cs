
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class OpenClaimCommandBaseResponse : BaseResponseType
    {
        public int BaseClaimKey { get; set; }
        public int ClaimKey { get; set; }
        public string ClaimNumber { get; set; }
        public bool PaymentAuthorized { get; set; }
        public string ResultingStatus { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
        public int Version { get; set; }
        public List<BaseClaimResponseTypeWarnings> Warnings { get; set; }
    }
}
