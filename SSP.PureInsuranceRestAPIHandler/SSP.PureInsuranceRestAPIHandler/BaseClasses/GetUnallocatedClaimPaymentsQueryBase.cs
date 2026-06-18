
using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetUnallocatedClaimPaymentsQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool PaymentDateSpecified { get; set; }
        public DateTime PaymentDateTo { get; set; }
        public bool PaymentDateToSpecified { get; set; }
        public string ShortCode { get; set; }
        public int AgentKey { get; set; }
    }
}
