namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBankPaymentType
    {
        public string AccountCode { get; set; }
        public string BIC { get; set; }
        public string BranchCode { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public bool ExpiryDateSpecified { get; set; }
        public string IBAN { get; set; }
        public int PartyBankKey { get; set; }
        public string PayeeName { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
    }
}
