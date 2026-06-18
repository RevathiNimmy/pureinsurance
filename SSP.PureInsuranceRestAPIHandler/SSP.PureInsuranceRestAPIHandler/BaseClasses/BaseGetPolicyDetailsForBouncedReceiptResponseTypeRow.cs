namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPolicyDetailsForBouncedReceiptResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }
        public string AccountShortcode { get; set; }
        public System.DateTime CoverEndDate { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public string DocumentRef { get; set; }
        public decimal GrossPremium { get; set; }
        public System.DateTime InceptionDate { get; set; }
        public int InsuranceFileKey { get; set; }
        public string InsuranceFileRef { get; set; }
        public int InsuredKey { get; set; }
        public string InsuredName { get; set; }
        public string InsuredShortcode { get; set; }
        public string PartyName { get; set; }
        public string PartyShortcode { get; set; }
        public string PartyType { get; set; }
    }
}
