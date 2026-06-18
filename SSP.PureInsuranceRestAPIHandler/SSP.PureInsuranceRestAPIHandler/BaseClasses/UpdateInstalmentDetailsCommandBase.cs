using System;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class UpdateInstalmentDetailsCommandBase : BaseRequestType
    {
        public DateTime DueDate { get; set; }
        public int FinancialPlanKey { get; set; }
        public int FinancialPlanVersion { get; set; }
        public int InstalmentNo { get; set; }
    }
}
