using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class PayClaimResponseType : BaseResponseType
    {
        public int baseClaimKey { get; set; }

        public int claimKey { get; set; }

        public string claimNumber { get; set; }

        public bool paymentAuthorized { get; set; }

        public string resultingStatus { get; set; }

        public byte[] timeStamp { get; set; }

        public int version { get; set; }

        public List<BaseClaimResponseTypeWarnings> warnings { get; set; }

        public BaseCashListResponseType cashList { get; set; }

        public int creditedAccountKey { get; set; }

        public int creditedDocumentKey { get; set; }

        public int creditedTransdetailKey { get; set; }

    }
}
