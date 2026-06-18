namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRiskReinsuranceBandsResponseTypeRow
    {
        //[DBCol("description")]
        public string Band { get; set; }
        //[DBCol("ri_band_id")]
        public int BandKey { get; set; }
    }
}
