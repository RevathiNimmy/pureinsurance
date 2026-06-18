using SSP.PureInsuranceRestAPIHandler.Enums;
using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCDTClaimPaymentType
    {
        public System.Collections.Generic.List<BaseCdtClaimPaymentItemType> ClaimPaymentItem { get; set; }
        public BaseCdtClaimReinsuranceType ClaimReinsurance { get; set; }
        public string CurrencyCode { get; set; }
        public int PartyKey { get; set; }
        public BaseClaimPayeeType Payee { get; set; }
        public ClaimPaymentPartyTypeType PaymentPartyType { get; set; }
        public BaseClaimReceiptPayeeType ReceiptPayee { get; set; }
        public int SAMStagingClaimPaymentKey { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public bool TransactionDateSpecified { get; set; }
        public int SiriusBaseReserveKey { get; set; }
    }
}
