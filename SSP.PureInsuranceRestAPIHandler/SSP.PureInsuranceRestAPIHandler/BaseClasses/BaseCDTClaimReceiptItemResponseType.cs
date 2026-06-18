namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimReceiptItemResponseType
    {
        public object SAMStagingClaimReceiptItemKeyField { get; set; }

        public object SiriusClaimReceiptItemKeyField { get; set; } = null;

        public BaseCdtClaimRiArrangementResponseType ClaimRiArrangementField { get; set; }
    }
}