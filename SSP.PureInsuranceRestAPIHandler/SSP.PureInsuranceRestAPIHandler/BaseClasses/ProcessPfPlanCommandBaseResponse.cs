namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ProcessPfPlan
{
    public class ProcessPfPlanCommandBaseResponse
    {
        public int PFPremFinanceKey { get; set; }
        public int PFPremFinanceVersion { get; set; }
        public int DepositTransdetailKey { get; set; }
        public string Warnings { get; set; }
    }
}
