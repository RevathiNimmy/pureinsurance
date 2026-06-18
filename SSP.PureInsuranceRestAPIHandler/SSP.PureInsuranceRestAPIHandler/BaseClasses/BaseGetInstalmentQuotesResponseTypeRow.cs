namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetInstalmentQuotesResponseTypeRow
    {
        //[DBCol("AgentCnt")]
        public int AgentCnt { get; set; }
        //[DBCol("AgentRef")]
        public string AgentRef { get; set; }
        //[DBCol("AlignTo")]
        public int AlignTo { get; set; }
        //[DBCol("AprRate")]
        public double AprRate { get; set; }
        //[DBCol("BankAddressMandatory")]
        public bool BankAddressMandatory { get; set; }
        //[DBCol("BankNameMandatory")]
        public bool BankNameMandatory { get; set; }
        //[DBCol("BranchCodeMandatory")]
        public bool BranchCodeMandatory { get; set; }
        //[DBCol("BranchNameMandatory")]
        public bool BranchNameMandatory { get; set; }
        //[DBCol("BrokerID")]
        public string BrokerID { get; set; }
        //[DBCol("BrokerURL")]
        public string BrokerURL { get; set; }
        //[DBCol("ClaimDebtID")]
        public int ClaimDebtID { get; set; }
        //[DBCol("CompanyName")]
        public string CompanyName { get; set; }
        //[DBCol("CompanyNo")]
        public int CompanyNo { get; set; }
        //[DBCol("DaysDelay")]
        public int DaysDelay { get; set; }
        //[DBCol("DepositAmount")]
        public double DepositAmount { get; set; }
        //[DBCol("DepositAsInstalment")]
        public bool DepositAsInstalment { get; set; }
        //[DBCol("FinanceCharge")]
        public double FinanceCharge { get; set; }
        //[DBCol("FinanceToNet")]
        public int FinanceToNet { get; set; }
        //[DBCol("FirstInstalmentAlignWithDayInMonth")]
        public int FirstInstalmentAlignWithDayInMonth { get; set; }
        //[DBCol("FirstInstalmentAmount")]
        public double FirstInstalmentAmount { get; set; }
        //[DBCol("FirstInstalmentDate")]
        public System.DateTime FirstInstalmentDate { get; set; }
        //[DBCol("FrequencyAmount")]
        public double FrequencyAmount { get; set; }
        //[DBCol("FrequencyDescription")]
        public string FrequencyDescription { get; set; }
        //[DBCol("FrequencyID")]
        public int FrequencyID { get; set; }
        //[DBCol("FrequencyPerYear")]
        public int FrequencyPerYear { get; set; }
        //[DBCol("FrequencyPeriod ")]
        public string FrequencyPeriod { get; set; }
        //[DBCol("HighlightCell")]
        public int HighlightCell { get; set; }
        //[DBCol("InstalmentsToPay")]
        public int InstalmentsToPay { get; set; }
        //[DBCol("InterestAmount")]
        public double InterestAmount { get; set; }
        //[DBCol("InterestRate")]
        public double InterestRate { get; set; }
        //[DBCol("IsUseTransactionCurrency")]
        public int IsUseTransactionCurrency { get; set; }
        //[DBCol("LastInstalmentAmount")]
        public double LastInstalmentAmount { get; set; }
        //[DBCol("LastInstalmentDate")]
        public System.DateTime LastInstalmentDate { get; set; }
        //[DBCol("MediaTypeDescription")]
        public string MediaTypeDescription { get; set; }
        //[DBCol("MediaTypeID")]
        public int MediaTypeID { get; set; }
        //[DBCol("MediaTypeValidation")]
        public string MediaTypeValidation { get; set; }
        //[DBCol("MinMTA")]
        public int MinMTA { get; set; }
        //[DBCol("NextInstalmentDate")]
        public System.DateTime NextInstalmentDate { get; set; }
        //[DBCol("OriginalAmount")]
        public double OriginalAmount { get; set; }
        //[DBCol("OriginalOtherInstalmentAmount")]
        public double OriginalOtherInstalmentAmount { get; set; }
        //[DBCol("OriginalRate")]
        public double OriginalRate { get; set; }
        //[DBCol("OtherInstalmentAmount")]
        public double OtherInstalmentAmount { get; set; }
        //[DBCol("PFRF_ID")]
        public int PFRF_ID { get; set; }
        //[DBCol("Password")]
        public string Password { get; set; }
        //[DBCol("ProductClass")]
        public string ProductClass { get; set; }
        //[DBCol("ProductCode")]
        public string ProductCode { get; set; }
        //[DBCol("ProtectionAmount")]
        public double ProtectionAmount { get; set; }
        //[DBCol("ProviderCode")]
        public string ProviderCode { get; set; }
        //[DBCol("Ref")]
        public string Ref { get; set; }
        //[DBCol("RefundType")]
        public int RefundType { get; set; }
        //[DBCol("SchemeName")]
        public string SchemeName { get; set; }
        //[DBCol("SchemeNo")]
        public int SchemeNo { get; set; }
        //[DBCol("SchemeTypeCode")]
        public string SchemeTypeCode { get; set; }
        //[DBCol("SchemeVersion")]
        public int SchemeVersion { get; set; }
        //[DBCol("SingleInstalmentPerMonth")]
        public int SingleInstalmentPerMonth { get; set; }
        //[DBCol("StartLimit")]
        public int StartLimit { get; set; }
        //[DBCol("TaxAmount")]
        public double TaxAmount { get; set; }
        //[DBCol("Terms")]
        public string Terms { get; set; }
        //[DBCol("Timeout")]
        public int Timeout { get; set; }
        //[DBCol("TotalAmountInput")]
        public double TotalAmountInput { get; set; }
        //[DBCol("TotalInstalmentsAmount")]
        public double TotalInstalmentsAmount { get; set; }
        //[DBCol("UseTransCurrncy")]
        public int UseTransCurrncy { get; set; }
        //[DBCol("UserID")]
        public string UserID { get; set; }
        //[DBCol("Username")]
        public string Username { get; set; }
    }
}
