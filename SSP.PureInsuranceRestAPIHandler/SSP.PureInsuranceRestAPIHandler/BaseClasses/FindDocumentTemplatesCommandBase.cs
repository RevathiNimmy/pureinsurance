namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindDocumentTemplatesCommandBase : BaseRequestType
    {
        public string Code { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public bool EffectiveDateSpecified { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public string ObjectName { get; set; }
        public string ProductCode { get; set; }
        public string PropertyName { get; set; }
        public string RiskTypeCode { get; set; }
        public string TypeCode { get; set; }
        public bool ViaClientManager { get; set; }
    }
}
