namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBindQuoteRequestDataStore
    {
        public int CurrencyId { get; set; }
        public int LeadAgnetKey { get; set; }
        public string RenewalStatusTypeCode { get; set; }
        public string TransferPartyShortname { get; set; }
        public bool AgentIsInTransferMode { get; set; }
        public bool PrintRenewalDebitNote { get; set; }
        public bool PrintRenewalSchedule { get; set; }
        public bool PrintRenewalCertificate { get; set; }
        public bool AnniversaryCopy { get; set; }
        public int RenewalStatusOriginalInsuranceFileKey { get; set; }
        public int RenewalStatusKey { get; set; }
        public string PartyShortname { get; set; }
        public string AgentShortname { get; set; }
        public string AgentType { get; set; }
        public decimal RefundAmount { get; set; }
        public bool CreateWorkManagerTaskForMTAReturnPremium { get; set; }
        public decimal TotalAgentCommission { get; set; }
        public decimal TotalAgentCommissionTax { get; set; }
        public decimal TotalAgentPremium { get; set; }
        public bool PolicyHasAgent { get; set; }
        public int TransactionTypeId { get; set; }
        public int ProductId { get; set; }
        public int OriginalInsuranceFileKey { get; set; }
        public string OriginalInsuranceFileStatus { get; set; }
        public int PFPremiumFinanceCnt { get; set; }
        public int PFPremiumFinanceVersion { get; set; }
        public object PFTransactions { get; set; }
        public int SourceId { get; set; }
        public decimal TotalPremiumAmount { get; set; }
        public string InsuranceRef { get; set; }
        public string PaymentMethod { get; set; }
        public int PartyKey { get; set; }
        public int InsuranceFolderKey { get; set; }
        public int TrueMonthlyPolicyIndicator { get; set; }
        public bool IsTrueMonthlyPolicy { get; set; }
        public System.DateTime CoverStartDate { get; set; }
        public bool IsBackDatedMTA { get; set; }
        public BaseLivePremiumFinancePlans[] LiveFinancePlans { get; set; }
        public bool IsPrePayment { get; set; }
        public int PaymentTerms { get; set; }
        public int CollectionFrequency { get; set; }
    }
}
