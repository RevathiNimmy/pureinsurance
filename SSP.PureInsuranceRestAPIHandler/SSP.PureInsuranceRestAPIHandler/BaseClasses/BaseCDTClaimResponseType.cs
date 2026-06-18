namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtClaimResponseType
    {
        public int SAMStagingClaimKey { get; set; }
        public int SiriusClaimKey { get; set; }
        public BaseCdtClaimPerilResponseType ClaimPeril { get; set; }
        public BaseCdtClaimRiArrangementResponseType ClaimRiArrangementResponseType { get; set; }
        public BaseCdtClaimResponseType ClaimField { get; set; }
    }
}
