using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class OpenClaimResponseType :BaseResponseType
    {

        public int baseClaimKey { get; set; }

        public int claimKey { get; set; }

        public string claimNumber { get; set; }

        public bool paymentAuthorized { get; set; }

        public string resultingStatus { get; set; }

        public byte[] timeStamp { get; set; }

        public int version { get; set; }

        public List<BaseClaimResponseTypeWarnings> warnings { get; set; }

    }
}
