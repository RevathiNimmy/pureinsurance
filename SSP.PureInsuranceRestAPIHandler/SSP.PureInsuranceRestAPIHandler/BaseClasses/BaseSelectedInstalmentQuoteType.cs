namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseSelectedInstalmentQuoteType
    {
        public double AmountPaid { get; set; }
        public double AmountToFinance { get; set; }
        public string BIC { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public BaseAddressType BankAddress { get; set; }
        public string BankAreaCode { get; set; }
        public string BankBranch { get; set; }
        public string BankExtn { get; set; }
        public string BankFax { get; set; }
        public string BankFaxCode { get; set; }
        public string BankName { get; set; }
        public string BankPhone { get; set; }
        public string BankSortCode { get; set; }
        public BaseCreditCardType CreditCard { get; set; }
        public System.DateTime EndDate { get; set; }
        public string IBAN { get; set; }
        public double InstDepositAmount { get; set; }
        public bool IsUseTransactionCurrency { get; set; }
        public int MonthDay { get; set; }
        public double OverrideInterestRate { get; set; }
        public double OverrideRate { get; set; }
        public int PFRF_ID { get; set; }
        public int PartyBankKey { get; set; }
        public bool PaymentProtection { get; set; }
        public System.DateTime PreferredDate { get; set; }
        public System.DateTime QuoteDate { get; set; }
        public int SelectedSchemeNo { get; set; }
        public int SelectedSchemeVersion { get; set; }
        public System.DateTime StartDate { get; set; }
        public int WeekDay { get; set; }
    }
}
