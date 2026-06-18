using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseInstalmentPlanDetailsType
    {
        public int FinancePlanKey { get; set; }
        public int FinancePlanVersion { get; set; }
        public System.Collections.Generic.List<BaseInstalmentPlanDetailsInstalmentDetails> InstalmentDetails { get; set; }
    }
}
