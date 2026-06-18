namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimPerilResponseType
    {
        public int SAMStagingClaimPerilKey { get; set; }
        public int SiriusClaimPerilKey { get; set; }
        public BaseCdtReserveResponseType Reserve { get; set; }
        public BaseCdtRecoveryResponseType Recovery { get; set; }
        public BaseCdtClaimPaymentResponseType ClaimPayment { get; set; }
        public BaseCdtClaimReceiptResponseType ClaimReceipt { get; set; }

    }
}
