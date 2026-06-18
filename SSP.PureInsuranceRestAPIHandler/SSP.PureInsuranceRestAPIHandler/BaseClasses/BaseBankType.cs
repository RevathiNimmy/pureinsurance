namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBankType
    {
        public string AccountNumber { get; set; }
        public string BIC { get; set; }
        public BaseSimpleAddressType BankAddress { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Branch { get; set; }
        public string BranchCode { get; set; }
        public string IBAN { get; set; }
        public int BankKey { get; set; }
    }
}
