namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.GetAccountDetails
{
    public class GetAccountDetailsQueryBase : BaseRequestType
    {
        public int AccountKey { get; set; }
        public bool AccountKeySpecified { get; set; }
        public string AltReference { get; set; }
        public string BGRef { get; set; }
        public int CashListKey { get; set; }
        public bool CashListKeySpecified { get; set; }
        public double CurrencyAmount { get; set; }
        public bool CurrencyAmountSpecified { get; set; }
        public string CurrencyCode { get; set; }
        public System.DateTime DateFrom { get; set; }
        public bool DateFromSpecified { get; set; }
        public System.DateTime DateTo { get; set; }
        public bool DateToSpecified { get; set; }
        public string Department { get; set; }
        public bool Display500 { get; set; }
        public bool Display500Specified { get; set; }
        public string DocTypeGroupCode { get; set; }
        public int DocumentKey { get; set; }
        public bool DocumentKeySpecified { get; set; }
        public string DocumentRef { get; set; }
        public string DocumentTypeCode { get; set; }
        public System.DateTime DueDateFrom { get; set; }
        public bool DueDateFromSpecified { get; set; }
        public System.DateTime DueDateTo { get; set; }
        public bool DueDateToSpecified { get; set; }
        public int FinancePlanKey { get; set; }
        public bool FinancePlanKeySpecified { get; set; }
        public bool IncludeReversedTran { get; set; }
        public bool IncludeReversedTranSpecified { get; set; }
        public string InsuranceRef { get; set; }
        public int InsuredAccountKey { get; set; }
        public bool InsuredAccountKeySpecified { get; set; }
        public bool IsLeadAgent { get; set; }
        public bool IsNewPF { get; set; }
        public bool IsNewPFSpecified { get; set; }
        public bool IsNewPFSpecified1 { get; set; }
        public bool IsSplitReceipt { get; set; }
        public bool OrderBySpare { get; set; }
        public bool OrderBySpareSpecified { get; set; }
        public bool OutstandingOnly { get; set; }
        public bool OutstandingOnlySpecified { get; set; }
        public bool OutstandingOnlySpecified1 { get; set; }
        public int PartyCnt { get; set; }
        public bool PartyCntSpecified { get; set; }
        public int PeriodKey { get; set; }
        public bool PeriodKeySpecified { get; set; }
        public string PurchaseInvoiceNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public bool Rollup { get; set; }
        public bool RollupSpecified { get; set; }
        public string SourceArray { get; set; }
        public string Spare { get; set; }
        public double Tolerance { get; set; }
        public bool ToleranceSpecified { get; set; }
        public bool ToleranceSpecified1 { get; set; }
        public string TransDetailKeys { get; set; }
        public int UnderwritingYearKey { get; set; }
        public bool UnderwritingYearKeySpecified { get; set; }
        public string Username { get; set; }
        public int AgentCnt { get; set; }
        public string AgentCode { get; set; }
        
        [Newtonsoft.Json.JsonIgnore]
        public int AgentKey { get; set; }
    }
}
