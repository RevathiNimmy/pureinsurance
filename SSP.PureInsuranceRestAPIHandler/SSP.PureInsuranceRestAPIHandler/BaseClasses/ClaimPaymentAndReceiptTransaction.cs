namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ClaimPaymentAndReceiptTransaction
    {
        public BaseCdtClaimType Claim { get; set; }
        public BaseCdtClaimPerilType ClaimPeril { get; set; }
        public BaseCDTClaimPaymentType ClaimPayment { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public BaseCdtClaimReceiptType ClaimReceipt { get; set; }
    }
}
