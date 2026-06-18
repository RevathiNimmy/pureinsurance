namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimReinsuranceBandsResponseTypeRow
    {
        //[DBCol("description")]
        public string Band { get; set; }
        //[DBCol("ri_band_id")]
        public int BandId { get; set; }
    }
}
