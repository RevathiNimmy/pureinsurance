namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimPaymentItemResponseType
    {
        public object SAMStagingClaimPaymentItemKeyField { get; set; }

        public object SiriusClaimPaymentItemKeyField { get; set; }

        public BaseCdtClaimRiArrangementResponseType ClaimRiArrangementField { get; set; }
    }
}