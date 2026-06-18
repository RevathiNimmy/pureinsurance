namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseFindInsuranceFileResponseTypeRow
    {
        //[DBCol("LapseDate")]
        public System.DateTime LapseDate { get; set; }
        //[DBCol("InsuranceFileTypeCode")]
        public string InsuranceFileTypeCode { get; set; }
        //[DBCol("InceptionDate")]
        public System.DateTime InceptionDate { get; set; }
        //[DBCol("LeadAgentKey")]
        public int LeadAgentKey { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("StatusId")]
        public int StatusId { get; set; }
        //[DBCol("IsSourceClosed")]
        public int IsSourceClosed { get; set; }
        //[DBCol("InsuranceRef")]
        public string InsuranceRef { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("RenewalDate")]
        public System.DateTime RenewalDate { get; set; }
        //[DBCol("RiskIndex")]
        public string RiskIndex { get; set; }
        //[DBCol("ClientShortName")]
        public string ClientShortName { get; set; }
        //[DBCol("ClientName")]
        public string ClientName { get; set; }
        //[DBCol("ClientAddressLine1")]
        public string ClientAddressLine1 { get; set; }
        //[DBCol("ClientPostCode")]
        public string ClientPostCode { get; set; }
        //[DBCol("InsuranceFileStatusCode")]
        public string InsuranceFileStatusCode { get; set; }
        //[DBCol("CoverFrom")]
        public System.DateTime CoverFrom { get; set; }
        //[DBCol("CoverTo")]
        public System.DateTime CoverTo { get; set; }
        //[DBCol("LeadAgentName")]
        public string LeadAgentName { get; set; }
        //[DBCol("AllowedClosedBranchClaims")]
        public int AllowedClosedBranchClaims { get; set; }
        public string ClaimStatus { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("DOBirth")]
        public System.DateTime DOB { get; set; }
        //[DBCol("FileCode")]
        public string FileCode { get; set; }
    }
}
