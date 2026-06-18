namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseRiskType : BaseRequestType
    {
        public string DataModelCode { get; set; }
        public byte[] QuoteTimeStamp { get; set; }
        public string RiskDescription { get; set; }
        public string RiskTypeCode { get; set; }
        public bool RunDefaultRules { get; set; }
        public string ScreenCode { get; set; }
        public string XMLDataSet { get; set; }
        public BaseProductBuilderRiskType[] ProductBuilderDetail { get; set; }
        public BaseTaxesAndFeesType[] TaxesAndFees { get; set; }
    }
}
