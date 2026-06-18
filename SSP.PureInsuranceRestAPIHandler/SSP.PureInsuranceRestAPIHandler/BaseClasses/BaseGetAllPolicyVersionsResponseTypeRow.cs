namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAllPolicyVersionsResponseTypeRow
    {
        //[DBCol("AgentKey")]
        public int AgentKey { get; set; }
        //[DBCol("AgentName")]
        public string AgentName { get; set; }
        //[DBCol("AlternativeRef")]
        public string AlternativeRef { get; set; }
        //[DBCol("AssociatedClients")]
        public string AssociatedClients { get; set; }
        //[DBCol("BaseInsuranceFileKey")]
        public int BaseInsuranceFileKey { get; set; }
        //[DBCol("CoverStartDate")]
        public System.DateTime CoverStartDate { get; set; }
        //[DBCol("Currency")]
        public string Currency { get; set; }
        //[DBCol("DateIssued")]
        public System.DateTime DateIssued { get; set; }
        public string EventDesc { get; set; }
        //[DBCol("ExpiryDate")]
        public System.DateTime ExpiryDate { get; set; }
        //[DBCol("GracePeriod")]
        public int GracePeriod { get; set; }
        //[DBCol("InstalmentPlanStatus")]
        public string InstalmentPlanStatus { get; set; }
        //[DBCol("InsuranceFileTypeCode")]
        public string InsuranceFileTypeCode { get; set; }
        //[DBCol("InsuranceFileTypeDesc")]
        public string InsuranceFileTypeDesc { get; set; }
        //[DBCol("InsuranceFileTypeKey")]
        public int InsuranceFileTypeKey { get; set; }
        //[DBCol("InsuranceFolderKey")]
        public int InsuranceFolderKey { get; set; }
        //[DBCol("InsuranceHolderKey")]
        public int InsuranceHolderKey { get; set; }
        //[DBCol("InsuredPersons")]
        public string InsuredPersons { get; set; }
        //[DBCol("Intermediary")]
        public string Intermediary { get; set; }
        //[DBCol("IsCurrent")]
        public int IsCurrent { get; set; }
        //[DBCol("IsMarketPlacePolicy")]
        public bool IsMarketPlacePolicy { get; set; }
        //[DBCol("IsMigratedPolicy")]
        public bool IsMigratedPolicy { get; set; }
        //[DBCol("IsOutOfSequenceReplaced")]
        public bool IsOutOfSequenceReplaced { get; set; }
        //[DBCol("IsReadOnly")]
        public bool IsReadOnly { get; set; }
        //[DBCol("LapseCancelDate")]
        public System.DateTime LapseCancelDate { get; set; }
        //[DBCol("MarkedQuoteForCollection")]
        public bool MarkedQuoteForCollection { get; set; }
        //[DBCol("PartyShortName")]
        public string PartyShortName { get; set; }
        //[DBCol("PaymentMethod")]
        public string PaymentMethod { get; set; }
        //[DBCol("PolicyRef")]
        public string PolicyRef { get; set; }
        //[DBCol("PolicyStatus")]
        public string PolicyStatus { get; set; }
        //[DBCol("PolicyStatusCode")]
        public string PolicyStatusCode { get; set; }
        //[DBCol("PolicyTypeCode")]
        public string PolicyTypeCode { get; set; }
        //[DBCol("PolicyVersion")]
        public string PolicyVersionDisplay { get; set; }
        //[DBCol("Premium")]
        public double Premium { get; set; }
        //[DBCol("PremiumSpecified")]
        public bool PremiumSpecified { get; set; }
        //[DBCol("PreviousVersionInstalmentPlanStatus")]
        public string PreviousVersionInstalmentPlanStatus { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProductDesc")]
        public string ProductDesc { get; set; }
        //[DBCol("QuoteExpiryDate")]
        public System.DateTime QuoteExpiryDate { get; set; }
        public bool QuoteExpiryDateSpecified { get; set; }
        //[DBCol("Regarding")]
        public string Regarding { get; set; }
        //[DBCol("RenewalDate")]
        public System.DateTime RenewalDate { get; set; }
        //[DBCol("TaxAmount")]
        public double TaxAmount { get; set; }
        public bool TaxAmountSpecified { get; set; }
        //[DBCol("TransactionDate")]
        public System.DateTime TransactionDate { get; set; }
        //[DBCol("insuranceFileKey")]
        public int insuranceFileKey { get; set; }
    }
}
