namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPoliciesInRenewalResponseTypeRow
    {
        //[DBCol("AccHandler")]
        public string AccHandler { get; set; }
        //[DBCol("AnniversaryCopy")]
        public bool AnniversaryCopy { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("ClaimIndicator")]
        public bool ClaimIndicator { get; set; }
        //[DBCol("CoverEndDate")]
        public System.DateTime CoverEndDate { get; set; }
        //[DBCol("CoverStartDate")]
        public System.DateTime CoverStartDate { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFileRef")]
        public string InsuranceFileRef { get; set; }
        //[DBCol("InsuranceFileStatusDescription")]
        public string InsuranceFileStatusDescription { get; set; }
        //[DBCol("InsuranceFileTypeDescription")]
        public string InsuranceFileTypeDescription { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("IsClosed")]
        public bool IsClosed { get; set; }
        //[DBCol("IsMarketPlacePolicy")]
        public bool IsMarketPlacePolicy { get; set; }
        //[DBCol("IsMigratedPolicy")]
        public bool IsMigratedPolicy { get; set; }
        //[DBCol("IsTrueMonthlyPolicy")]
        public bool IsTrueMonthlyPolicy { get; set; }
        //[DBCol("LeadAgent")]
        public string LeadAgent { get; set; }
        //[DBCol("LeadAgentKey")]
        public int LeadAgentKey { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("PartyName")]
        public string PartyName { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductDescription")]
        public string ProductDescription { get; set; }
        //[DBCol("RenewalDate")]
        public System.DateTime RenewalDate { get; set; }
        //[DBCol("RenewalPremium")]
        public decimal RenewalPremium { get; set; }
        //[DBCol("RenewalStatusKey")]
        public int RenewalStatusKey { get; set; }
        //[DBCol("RenewalStatusTypeCode")]
        public string RenewalStatusTypeCode { get; set; }
        //[DBCol("RenewalStatusTypeDescription")]
        public string RenewalStatusTypeDescription { get; set; }
    }
}
