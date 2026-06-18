namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimPerilRecoveryType
    {
        public int BaseRecoveryKey { get; set; }
        public decimal Initialamount { get; set; }
        public bool IsDeletedRecovery { get; set; }
        public bool IsNew { get; set; }
        public decimal RevisionAmount { get; set; }
        public string TypeCode { get; set; }
        public int RecoveryTypeId { get; set; }
        public int RecoveryId { get; set; }
        public bool RecoveryPartyKeySpecified { get; set; }
        public bool RecoveryPartyTypeKeySpecified { get; set; }
        public int RecoveryPartyKey { get; set; }
        public int RecoveryPartyTypeKey { get; set; }
        public string RecoveryPartyCode { get; set; }
    }
}
