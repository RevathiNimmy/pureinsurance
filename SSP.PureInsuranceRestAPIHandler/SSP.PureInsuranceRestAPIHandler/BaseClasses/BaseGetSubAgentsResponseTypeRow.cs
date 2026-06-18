namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetSubAgentsResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("CommissionValue")]
        public double Amount { get; set; }

        //[DBCol("shortname")]
        public string Code { get; set; }

        //[DBCol("resolved_name")]
        public string Name { get; set; }

        //[DBCol("party_cnt")]
        public int PartyKey { get; set; }

        //[DBCol("CommissionPC")]
        public double Percentage { get; set; }
    }
}
