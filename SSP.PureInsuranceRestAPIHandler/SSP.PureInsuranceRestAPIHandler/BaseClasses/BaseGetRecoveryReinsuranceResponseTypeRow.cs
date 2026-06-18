namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRecoveryReinsuranceResponseTypeRow
    {
        //[DBCol("party_cnt")]
        public int PartyKey { get; set; }
        //[DBCol("recovery")]
        public decimal Recovery { get; set; }
        //[DBCol("recovery_id")]
        public int RecoveryKey { get; set; }
        //[DBCol("to_date")]
        public decimal RecoveryToDate { get; set; }
        //[DBCol("description")]
        public string RecoveryType { get; set; }
        //[DBCol("name")]
        public string Reinsurer { get; set; }
        //[DBCol("salvage")]
        public decimal Salvage { get; set; }
        //[DBCol("this_share_percent")]
        public decimal SharePercent { get; set; }
        //[DBCol("this_recovery")]
        public decimal ThisRecovery { get; set; }
        //[DBCol("this_salvage")]
        public decimal ThisSalvage { get; set; }
    }
}
