namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetHeaderAndSummariesResponseTypeRow
    {
        //[DBCol("RiskKey")]
        public int RiskKey { get; set; }

        //[DBCol("RiskFolderKey")]
        public int RiskFolderKey { get; set; }

        //[DBCol("RiskTypeCode")]
        public string RiskTypeCode { get; set; }

        //[DBCol("Description")]
        public string Description { get; set; }

        //[DBCol("TotalSumInsured")]
        public double TotalSumInsured { get; set; }

        //[DBCol("TotalSumInsuredSpecified")]
        public bool TotalSumInsuredSpecified { get; set; }

        //[DBCol("Premium")]
        public double Premium { get; set; }

        //[DBCol("PremiumSpecified")]
        public bool PremiumSpecified { get; set; }

        //[DBCol("StatusCode")]
        public string StatusCode { get; set; }

        //[DBCol("GISRetroactiveDate")]
        public System.DateTime GISRetroactiveDate { get; set; }

        //[DBCol("RiskInceptionDate")]
        public System.DateTime RiskInceptionDate { get; set; }

        //[DBCol("HasClaimLink")]
        public bool HasClaimLink { get; set; }

        //[DBCol("CollectionFrequency")]
        public int CollectionFrequency { get; set; }

        //[DBCol("PaymentTerms")]
        public int PaymentTerms { get; set; }

        //[DBCol("RiskLinkStatusFlag")]
        public string RiskLinkStatusFlag { get; set; }

        //[DBCol("RiskLinkChangeDate")]
        public System.DateTime RiskLinkChangeDate { get; set; }

        //[DBCol("OriginalRiskKey")]
        public int OriginalRiskKey { get; set; }

        //[DBCol("IsEditable")]
        public bool IsEditable { get; set; }

        //[DBCol("HasFacProp")]
        public bool HasFacProp { get; set; }

        //[DBCol("IsAutoRated")]
        public bool IsAutoRated { get; set; }
        //[DBCol("RiskNumber")]
        public int RiskNumber { get; set; }

    }
}
