using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimReceiptType
    {
        public System.Collections.Generic.List<BaseCdtReceiptItemType> ClaimReceiptItem { get; set; }
        public BaseCdtClaimReinsuranceType ClaimReinsurance { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsSalvageRecovery { get; set; }
        public int PartyKey { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public ClaimReceiptPartyTypeType ReceiptPartyType { get; set; }
        public int SAMStagingClaimReceiptKey { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public bool TransactionDateSpecified { get; set; }
        public BaseClaimReceiptPayeeType payeeReceipt { get; set; }
    }
}
