using SSP.PureInsuranceRestAPIHandler.Enums;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessReceiptDetailsType
    {
        public int ReceiptPartyKey { get; set; }
        public string ReceiptMediaTypeCode { get; set; }
        public string ReceiptMediaReference { get; set; }
        public string ReceiptPayee { get; set; }
        public string ReceiptBankCode { get; set; }
        public string ReceiptCurrencyCode { get; set; }
        public ClaimReceiptPartyTypeType ReceiptPartyType { get; set; }
    }
}
