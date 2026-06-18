namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetCoinsuranceDefaultsResponseTypeRow
    {
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }

        //[DBCol("code")]
        public string Code { get; set; }

        //[DBCol("description")]
        public string CoinsuranceDefault { get; set; }

        //[DBCol("coi_default_id")]
        public int CoinsuranceDefaultId { get; set; }

        //[DBCol("is_recovered")]
        public bool IsRecovered { get; set; }

        //[DBCol("is_surcharged")]
        public bool IsSurcharged { get; set; }
    }
}
