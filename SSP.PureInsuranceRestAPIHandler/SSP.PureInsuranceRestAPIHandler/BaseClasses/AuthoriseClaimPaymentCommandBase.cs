using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class AuthoriseClaimPaymentCommandBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public int ClaimPaymentKey { get; set; }
        public string Comments { get; set; }
        public bool Declined { get; set; }
        public bool ExclusiveLock { get; set; }
        public bool IsRecommended { get; set; }
        public BasePaymentCashListType PaymentCashList { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool PaymentDateSpecified { get; set; }
        public DateTime PaymentDateTo { get; set; }
        public bool PaymentDateToSpecified { get; set; }
        public string RecommendedBy { get; set; }
        public string SessionValue { get; set; }
        public string ShortCode { get; set; }
        public string SourceArray { get; set; }
        public byte[] ApiTimeStamp { get; set; } = new byte[0];
    }
}
