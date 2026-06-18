namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseProcessPFPlanRequestTrans
    {
        public int TransdetailKey { get; set; }
        public string PolicyRef { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Spare { get; set; }
        public int DocumentTypeId { get; set; }
        public int InsuranceFileKey { get; set; }
        public int AccountId { get; set; }
    }
}
