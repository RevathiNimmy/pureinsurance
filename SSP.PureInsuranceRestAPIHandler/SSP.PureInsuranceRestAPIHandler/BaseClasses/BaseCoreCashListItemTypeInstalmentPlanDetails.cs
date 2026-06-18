namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCoreCashListItemTypeInstalmentPlanDetails
    {
        public int FinancePlanKey { get; set; }
        public int FinancePlanVersion { get; set; }
        public BaseCoreCashListItemTypeInstalmentPlanDetailsInstalmentDetails[] InstalmentDetails { get; set; }
    }
}
