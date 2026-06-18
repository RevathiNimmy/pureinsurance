namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseInstalmentPlanDetailsInstalmentDetails
    {
        public double ActualAmount { get; set; }
        public double Amount { get; set; }
        public int InstalmentNumber { get; set; }
        public bool IsPartialPayment { get; set; }
        public bool IsWriteOffPayment { get; set; }
        public double OverPaymentWriteOffAmount { get; set; }
        public int WriteOffReasonID { get; set; }
        public int PFInstalmentID { get; set; }
    }
}
