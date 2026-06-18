namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseCdtRecoveryType
    {
        public decimal RevisionAmount { get; set; }
        public int SAMStagingRecoveryKey { get; set; }
        public int SiriusBaseRecoveryKey { get; set; }
        public string TypeCode { get; set; }
        public int RecoveryPartyKey { get; set; }
        public string RecoveryPartyTypeCode { get; set; }
    }
}
