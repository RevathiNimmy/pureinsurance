namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimReceiptPayeeType
    {
        public BaseAddressType Address { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Comments { get; set; }
        public string MediaReference { get; set; }
        public string MediaTypeCode { get; set; }
        public string Name { get; set; }
        public int PartyBankKey { get; set; }
        public string TheirReference { get; set; }
    }
}
