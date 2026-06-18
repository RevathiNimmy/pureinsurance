namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePartyBankHistoryType
    {
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string ActionCode { get; set; }
        public string BIC { get; set; }
        public string BankBranch { get; set; }
        public string BankBranchCode { get; set; }
        public string BankName { get; set; }
        public System.DateTime DateModified { get; set; }
        public string IBAN { get; set; }
        public int PartyBankKey { get; set; }
        public string PostCode { get; set; }
        public string StreetName { get; set; }
        public string UserName { get; set; }
    }
}
