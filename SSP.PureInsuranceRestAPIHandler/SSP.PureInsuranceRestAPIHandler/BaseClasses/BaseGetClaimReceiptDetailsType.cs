using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReceiptDetailsType
    {
        public int ClaimPerilId { get; set; }
        public int BaseClaimReceiptKey { get; set; }
        public System.DateTime ReceiptDate { get; set; }
        public int PartyKey { get; set; }
        public string ReceiptPartyType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string ReceiptPartyCode { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public BaseClaimReceiptAdvancedTaxDetailsType AdvancedTax { get; set; }
        public System.Collections.Generic.List<BaseGetClaimReceiptItemDetailsType> ReceiptItem { get; set; }
        public int ClaimReceiptKey { get; set; }
    }

}