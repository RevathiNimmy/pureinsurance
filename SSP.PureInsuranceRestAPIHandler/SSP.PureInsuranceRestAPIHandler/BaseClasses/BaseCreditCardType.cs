namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCreditCardType
    {
        public string AccountType { get; set; }
        public string AuthCode { get; set; }
        public BaseCreditCardTypeCardHolder CardHolder { get; set; }
        public string CashListItemBankCode { get; set; }
        public bool CustomerPresent { get; set; }
        public string ExpiryDate { get; set; }
        public bool IsDefaultCreditCard { get; set; }
        public bool IsRegisteredCardHolder { get; set; }
        public string Issue { get; set; }
        public string ManualAuthCode { get; set; }
        public string NameOnCreditCard { get; set; }
        public string Number { get; set; }
        public int PartyBankKey { get; set; }
        public string Pin { get; set; }
        public string StartDate { get; set; }
        public string TrackingNumber { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionSlipNumber { get; set; }
        public string TypeCode { get; set; }
        public bool VIAPaymentHub { get; set; }

        public bool IsValidated { get; set; }

    }
}
