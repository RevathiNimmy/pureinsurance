namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class DefaultRules
    {
        // In ONLY Parameters
        public string DataModelCode { get; set; }
        public string BusinessTypeCode { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int GISSchemeID { get; set; }
        public int RiskGroupID { get; set; }
        public int RiskScreenId { get; set; }

        // In/Outs
        public string XMLDataset { get; set; }
        public AdditionalData[] AdditionalDataArray { get; set; }
    }
}
