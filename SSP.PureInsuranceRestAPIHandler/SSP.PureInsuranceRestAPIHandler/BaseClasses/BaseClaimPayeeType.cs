namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPayeeType
    {
        public BaseAddressType Address { get; set; }
        public string BIC { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Comments { get; set; }
        public string IBAN { get; set; }
        public string MediaReference { get; set; }
        public string MediaTypeCode { get; set; }
        public string MediaTypeDesc { get; set; }
        public string Name { get; set; }
        public int PartyBankKey { get; set; }
        public string TheirReference { get; set; }

        public int MediaTypeId { get; set; }

        public System.DateTime Chequedate { get; set; }
        public string AccountType { get; set; }
    }
}
