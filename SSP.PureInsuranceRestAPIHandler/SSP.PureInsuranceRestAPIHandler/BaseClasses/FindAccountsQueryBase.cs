namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindAccountsQuery
{
    public class FindAccountsQueryBase : BaseRequestType
    {
        public string LedgerCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountTypeCode { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string InsuranceRef { get; set; } = string.Empty;
        public int OperatorKey { get; set; }
        public bool OperatorKeySpecified { get; set; }
        public string PurchaseOrderNo { get; set; } = string.Empty;
        public string PurchaseInvoiceNo { get; set; } = string.Empty;
        public string Spare { get; set; }
        public bool ShowDeleted { get; set; }
        public bool ShowBalance { get; set; }
        public bool OnlyUpdatableAccounts { get; set; }
        public bool IncludeInsurerAgents { get; set; }
        public bool ExcludeInsurerAgents { get; set; }
        public int MaxRowsToFetch { get; set; }
        public bool MaxRowsToFetchSpecified { get; set; }
        public int BrokerCount { get; set; }
    }
}
