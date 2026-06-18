namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimReceiptResponseType
    {
        public object SAMStagingClaimReceiptKeyField { get; set; }

        public object SiriusClaimReceiptKeyField { get; set; }
        public BaseCdtClaimReceiptItemResponseType ClaimReceiptItemField { get; set; }
    }
}