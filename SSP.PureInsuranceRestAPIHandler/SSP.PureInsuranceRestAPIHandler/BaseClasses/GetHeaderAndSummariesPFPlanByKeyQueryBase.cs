using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetHeaderAndSummariesPFPlanByKeyQueryBase : BaseRequestType
    {
        public string DocumentRef { get; set; }
        public int PFPremiumFinanceKey { get; set; }
        public int PFPremiumFinanceVersionKey { get; set; }
        public int UserID { get; set; }
    }
}
