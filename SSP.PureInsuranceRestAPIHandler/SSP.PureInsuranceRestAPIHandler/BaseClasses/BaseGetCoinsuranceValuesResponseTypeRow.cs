namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetCoinsuranceValuesResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("arrangement_ref")]
        public string ArrangementRef { get; set; }

        //[DBCol("name")]
        public string CoInsurer { get; set; }

        //[DBCol("party_cnt")]
        public int CoInsurerKey { get; set; }

        //[DBCol("commission_percent")]
        public double CommissionPerc { get; set; }

        //[DBCol("share_percent")]
        public double SharePerc { get; set; }
    }
}
