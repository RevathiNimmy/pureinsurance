namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class NBQuote
    {
        // In ONLY Parameters
        public string DataModelCode { get; set; }

        public string BusinessTypeCode { get; set; }

        public System.DateTime EffectiveDate { get; set; }

        public int GISSchemeID { get; set; }

        public int RiskGroupID { get; set; }

        public int RiskScreenId { get; set; }

        public string Username { get; set; }

        public bool isClaimValidation { get; set; }
        // In/Outs Parameters
        public string XMLDataset { get; set; }

        public AdditionalData[] AdditionalDataArray { get; set; }

        public int ClaimTransactiontypeId { get; set; }
    }
}
