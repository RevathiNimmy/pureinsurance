namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetPartySummaryResponseTypeRow
    {
        //[DBCol("AgentShortName")]
        public string AgentShortName { get; set; }
        //[DBCol("AlternativeRef")]
        public string AlternativeRef { get; set; }
        //[DBCol("AnnualPremium")]
        public double AnnualPremium { get; set; }
        //[DBCol("AnnualPremiumSpecified")]
        public bool AnnualPremiumSpecified { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("BaseInsuranceFolderKey")]
        public int BaseInsuranceFolderKey { get; set; }
        //[DBCol("BranchCode")]
        public string BranchCode { get; set; }
        //[DBCol("BranchKey")]
        public int BranchKey { get; set; }
        //[DBCol("CoverStartDate")]
        public System.DateTime CoverStartDate { get; set; }
        //[DBCol("CoverStartDateSpecified")]
        public bool CoverStartDateSpecified { get; set; }
        //[DBCol("CurrencyCode")]
        public string CurrencyCode { get; set; }
        //[DBCol("DateIssued")]
        public System.DateTime DateIssued { get; set; }
        //[DBCol("DateIssuedSpecified")]
        public bool DateIssuedSpecified { get; set; }
        //[DBCol("EventDescription")]
        public string EventDescription { get; set; }
        //[DBCol("ExpiryDate")]
        public System.DateTime ExpiryDate { get; set; }
        //[DBCol("ExpiryDateSpecified")]
        public bool ExpiryDateSpecified { get; set; }
        //[DBCol("GeminiPolicyStatus")]
        public int GeminiPolicyStatus { get; set; }
        //[DBCol("GeminiPolicyStatusSpecified")]
        public bool GeminiPolicyStatusSpecified { get; set; }
        //[DBCol("InsuranceFileId")]
        public int InsuranceFileId { get; set; }
        //[DBCol("InsuranceFileKey")]
        public int InsuranceFileKey { get; set; }
        //[DBCol("InsuranceFileTypeCode")]
        public string InsuranceFileTypeCode { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("InsuredKey")]
        public int InsuredKey { get; set; }
        //[DBCol("InsuredKeySpecified")]
        public bool InsuredKeySpecified { get; set; }
        //[DBCol("InsurerName")]
        public string InsurerName { get; set; }
        //[DBCol("InsurerShortName")]
        public string InsurerShortName { get; set; }
        //[DBCol("IsCurrent")]
        public bool IsCurrent { get; set; }
        //[DBCol("IsMarketPlacePolicy")]
        public bool IsMarketPlacePolicy { get; set; }
        //[DBCol("IsMigratedPolicy")]
        public bool IsMigratedPolicy { get; set; }
        //[DBCol("IsReadOnly")]
        public bool IsReadOnly { get; set; }
        //[DBCol("LeadAgentKey")]
        public int LeadAgentKey { get; set; }
        //[DBCol("LeadAgentKeySpecified")]
        public bool LeadAgentKeySpecified { get; set; }
        //[DBCol("LeadInsurerKey")]
        public int LeadInsurerKey { get; set; }
        //[DBCol("LeadInsurerKeySpecified")]
        public bool LeadInsurerKeySpecified { get; set; }
        //[DBCol("MarkedForCollection")]
        public bool MarkedForCollection { get; set; }
        //[DBCol("NetPremium")]
        public double NetPremium { get; set; }
        //[DBCol("NetPremiumSpecified")]
        public bool NetPremiumSpecified { get; set; }
        //[DBCol("PartyShortName")]
        public string PartyShortName { get; set; }
        //[DBCol("PolicyRef")]
        public string PolicyRef { get; set; }
        //[DBCol("PolicyStatus")]
        public string PolicyStatus { get; set; }
        //[DBCol("PolicyStatusCode")]
        public string PolicyStatusCode { get; set; }
        //[DBCol("PolicyTypeCode")]
        public string PolicyTypeCode { get; set; }
        //[DBCol("PolicyTypeDesc")]
        public string PolicyTypeDesc { get; set; }
        //[DBCol("PolicyTypeId")]
        public int PolicyTypeId { get; set; }
        //[DBCol("PolicyTypeIdSpecified")]
        public bool PolicyTypeIdSpecified { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductDesc")]
        public string ProductDesc { get; set; }
        //[DBCol("ProductKey")]
        public int ProductKey { get; set; }
        //[DBCol("QuoteExpiryDate")]
        public System.DateTime QuoteExpiryDate { get; set; }
        //[DBCol("QuoteStatusKey")]
        public int QuoteStatusKey { get; set; }
        //[DBCol("QuoteVersion")]
        public int QuoteVersion { get; set; }
        //[DBCol("Regarding")]
        public string Regarding { get; set; }
        //[DBCol("RenewalDate")]
        public System.DateTime RenewalDate { get; set; }
        //[DBCol("RenewalDateSpecified")]
        public bool RenewalDateSpecified { get; set; }
        //[DBCol("RenewedVersion")]
        public int RenewedVersion { get; set; }
        //[DBCol("RiskStatus")]
        public string RiskStatus { get; set; }
        //[DBCol("RiskTypeDescription")]
        public string RiskTypeDescription { get; set; }
        //[DBCol("TaxAmount")]
        public double TaxAmount { get; set; }
        //[DBCol("TaxAmountSpecified")]
        public bool TaxAmountSpecified { get; set; }
        //[DBCol("ThisPremium")]
        public double ThisPremium { get; set; }
        //[DBCol("ThisPremiumSpecified")]
        public bool ThisPremiumSpecified { get; set; }
    }
}
