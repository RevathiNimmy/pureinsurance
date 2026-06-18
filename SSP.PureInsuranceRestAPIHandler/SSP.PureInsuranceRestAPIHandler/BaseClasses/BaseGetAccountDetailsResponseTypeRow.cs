namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetAccountDetailsResponseTypeTransactions
    {
        public string Account { get; set; }
        public double AccountAmount { get; set; }
        public string AccountCurrencyCode { get; set; }
        public double AccountOutStandingAmount { get; set; }
        public int Accountkey { get; set; }
        public string AltRef { get; set; }
        public double Amount { get; set; }
        public string BGRef { get; set; }
        public string BalanceType { get; set; }
        public int BankAccountID { get; set; }
        public string BaseCurrencyCode { get; set; }
        public int BranchKey { get; set; }
        public int CashListItemKey { get; set; }
        public int CashListKey { get; set; }
        public string Client { get; set; }
        public string ClientCode { get; set; }
        public double CurrencyAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string DocRef { get; set; }
        public string DocTypeDescription { get; set; }
        public int DocTypeId { get; set; }
        public string DocumentComment { get; set; }
        public string DocumentGroupCode { get; set; }
        public int DocumentGroupId { get; set; }
        public string DocumentTypeCode { get; set; }
        public System.DateTime DueDate { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public int FinancePlanKey { get; set; }
        public string FinancePlanStatus { get; set; }
        public int FinancePlanVersion { get; set; }
        public bool InstalmentCollection { get; set; }
        public bool IsLeadAgent { get; set; }
        public bool IsSplitReceipt { get; set; }
        public string MediaRef { get; set; }
        public string MediaType { get; set; }
        public string OperatorName { get; set; }
        public double OutStandingCurrencyAmount { get; set; }
        public double OutstandingAmount { get; set; }
        public System.DateTime PaidDate { get; set; }
        public int PartyCnt { get; set; }
        public string PayeeName { get; set; }
        public string Period { get; set; }
        public bool PrimarySettled { get; set; }
        public string Reference { get; set; }
        public System.DateTime TransDate { get; set; }
        public int TransDetailKey { get; set; }
        public string UnderwritingYear { get; set; }
        public bool IsLead { get; set; }
        public int InsuranceFileCnt { get; set; }
        public int InsuranceFolderCnt { get; set; }
    }
}
