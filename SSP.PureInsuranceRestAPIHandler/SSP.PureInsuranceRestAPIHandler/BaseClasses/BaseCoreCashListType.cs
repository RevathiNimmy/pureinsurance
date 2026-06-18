namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCoreCashListType
    {
        public int BankAccountKey { get; set; }
        public int CashListKey { get; set; }
        public string TypeCode { get; set; }
        public System.DateTime ListDate { get; set; }
        public string BankAccountCode { get; set; }
        public string CurrencyCode { get; set; }
        public string Reference { get; set; }
        public string StatusCode { get; set; }
        public string BankAccountName { get; set; }
        public string SubBranchCode { get; set; }
        public string BranchCode { get; set; }

        public int SourceID { get; set; }

        public int TypeKey { get; set; }

        public int CurrencyKey { get; set; }

        public int StatusKey { get; set; }

        public int SubBranchID { get; set; }
    }
}
