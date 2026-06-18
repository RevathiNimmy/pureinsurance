using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class CancelPFPoliciesCommandBase : BaseRequestType
    {
        public string LapsedReasonCode { get; set; }
        public int PFPremiumFinanceKey { get; set; }
        public int PFPremiumFinanceVersionKey { get; set; }
        public DateTime PolicyLapsedDate { get; set; }
        public bool SpoolDoc { get; set; }
        public bool WriteOff { get; set; }
    }
}
