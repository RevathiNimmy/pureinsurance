namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePremiumFinancePlanInstalmentsHistoryType
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public string PFIResultCode { get; set; }
        public string PFIResultDescription { get; set; }
        public string PFIStatusDescription { get; set; }
        public System.DateTime PostedDate { get; set; }
    }
}
