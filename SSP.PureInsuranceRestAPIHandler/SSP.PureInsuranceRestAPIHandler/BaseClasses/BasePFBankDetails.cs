namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BasePFBankDetails
    {
        public int PartyBankKey { get; set; }
        public string BankName { get; set; }
        public string BankSortCode { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAccountName { get; set; }
        public string BankBranch { get; set; }
        public string BankAreaCode { get; set; }
        public string BankPhone { get; set; }
        public string BankExtn { get; set; }
        public string BankFaxCode { get; set; }
        public string BankFax { get; set; }
        public BaseAddressType BankAddress { get; set; }
    }
}
