using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetFinancePlanDetailsRequestType : BaseRequestType
    {
        public int FinancePlanKey { get; set; }
        public int FinancePlanVersion { get; set; }
        public int InsuranceFileKey { get; set; }
    }
}
