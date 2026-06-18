namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimRiArrangementResponseType
    {
        public object SAMStagingClaimRIArrangementKeyField { get; set; }
        public object SiriusClaimRIArrangementKeyField { get; set; }
        public BaseCdtClaimRIArrangmentLineResponseType ClaimRIArrangmentLineField { get; set; }
    }
}
