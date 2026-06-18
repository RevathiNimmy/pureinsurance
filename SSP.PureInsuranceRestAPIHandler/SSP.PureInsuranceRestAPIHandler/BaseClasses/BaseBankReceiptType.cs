namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseBankReceiptType
    {

        public int ChequeTypeKey { get; set; }

        public int ChequeClearingTypeKey { get; set; }
        public string BankBranch { get; set; }
        public string BankCode { get; set; }
        public string BankLocation { get; set; }
        public string ChequeClearingTypeCode { get; set; }
        public System.DateTime ChequeDate { get; set; }
        public string ChequeTypeCode { get; set; }
        public int PartyBankKey { get; set; }
        public string PayerName { get; set; }
    }
}
