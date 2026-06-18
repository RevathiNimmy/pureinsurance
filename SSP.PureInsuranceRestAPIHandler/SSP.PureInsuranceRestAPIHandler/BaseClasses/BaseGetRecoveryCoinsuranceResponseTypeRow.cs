namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRecoveryCoinsuranceResponseTypeRow
    {
        //[DBCol("Coinsurer")]
        public string Coinsurer { get; set; }
        //[DBCol("PartyKey")]
        public int PartyKey { get; set; }
        //[DBCol("RecoveryKey")]
        public int RecoveryKey { get; set; }
        //[DBCol("RecoveryToDate")]
        public decimal RecoveryToDate { get; set; }
        //[DBCol("RecoveryType")]
        public string RecoveryType { get; set; }
        //[DBCol("SharePercent")]
        public decimal SharePercent { get; set; }
        public string RecoveryTypeCode { get; set; }
    }
}
