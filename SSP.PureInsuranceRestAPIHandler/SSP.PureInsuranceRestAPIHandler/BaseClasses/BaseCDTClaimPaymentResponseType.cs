namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimPaymentResponseType
    {
        public object SAMStagingClaimPaymentKeyField { get; set; }

        public object SiriusClaimPaymentKeyField { get; set; }

        public BaseCdtClaimPaymentItemResponseType ClaimPaymentItemField { get; set; }

    }
}