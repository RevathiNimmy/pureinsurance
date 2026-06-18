namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindClaimResponseTypeRow
    {
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("BaseClaimKey")]
        public int BaseClaimKey { get; set; }
        //[DBCol("CaseNumber")]
        public string CaseNumber { get; set; }
        //[DBCol("CatastropheCode")]
        public string CatastropheCode { get; set; }
        //[DBCol("ClaimDescription")]
        public string ClaimDescription { get; set; }
        //[DBCol("ClaimHandlerDescription")]
        public string ClaimHandlerDescription { get; set; }
        //[DBCol("ClaimKey")]
        public int ClaimKey { get; set; }
        //[DBCol("ClaimNumber")]
        public string ClaimNumber { get; set; }
        //[DBCol("ClaimRiskField")]
        public string ClaimRiskField { get; set; }
        //[DBCol("ClaimStatus")]
        public string ClaimStatus { get; set; }
        //[DBCol("ClaimStatusID")]
        public int ClaimStatusID { get; set; }
        //[DBCol("ClientClaimNumber")]
        public string ClientClaimNumber { get; set; }
        //[DBCol("ClientName")]
        public string ClientName { get; set; }
        //[DBCol("ClientShortName")]
        public string ClientShortName { get; set; }
        //[DBCol("ClientTelephoneNumber")]
        public string ClientTelephoneNumber { get; set; }
        //[DBCol("ClientTelephoneNumberOffice")]
        public string ClientTelephoneNumberOffice { get; set; }
        //[DBCol("CoverFrom")]
        public System.DateTime CoverFrom { get; set; }
        //[DBCol("CoverTo")]
        public System.DateTime CoverTo { get; set; }
        //[DBCol("CurrencyISOCode")]
        public string CurrencyISOCode { get; set; }
        public bool InfoOnly { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("InsurerClaimNumber")]
        public string InsurerClaimNumber { get; set; }
        //[DBCol("IsAllowedClosedClaims")]
        public bool IsAllowedClosedClaims { get; set; }
        //[DBCol("IsDeleted")]
        public bool IsDeleted { get; set; }
        //[DBCol("LastModifiedDate")]
        public System.DateTime LastModifiedDate { get; set; }
        //[DBCol("LeadAgentName")]
        public string LeadAgentName { get; set; }
        //[DBCol("LossDateFrom")]
        public System.DateTime LossDateFrom { get; set; }
        //[DBCol("NotificationDate")]
        public System.DateTime NotificationDate { get; set; }
        //[DBCol("Payments")]
        public decimal Payments { get; set; }
        //[DBCol("PrimaryCauseDescription")]
        public string PrimaryCauseDescription { get; set; }
        //[DBCol("ProductDescription")]
        public string ProductDescription { get; set; }
        //[DBCol("ProgressStatusDescription")]
        public string ProgressStatusDescription { get; set; }
        //[DBCol("ReportedDate")]
        public System.DateTime ReportedDate { get; set; }
        //[DBCol("Reserve")]
        public decimal Reserve { get; set; }
        //[DBCol("RiskKey")]
        public int RiskKey { get; set; }
        //[DBCol("SearchResultsCol1")]
        public string SearchResultsCol1 { get; set; }
        //[DBCol("SecondaryCauseDescription")]
        public string SecondaryCauseDescription { get; set; }
        //[DBCol("RiskDescription")]
        public string RiskDescription { get; set; }
    }
}
