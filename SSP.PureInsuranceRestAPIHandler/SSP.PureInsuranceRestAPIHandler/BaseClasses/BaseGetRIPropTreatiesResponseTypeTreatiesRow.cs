namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRIPropTreatiesResponseTypeTreatiesRow
    {
        //[DBCol("code")]
        public string TreatyCode { get; set; }
        //[DBCol("description")]
        public string TreatyDescription { get; set; }
        //[DBCol("treaty_id")]
        public int TreatyId { get; set; }
        public string ReinsuranceCode { get; set; } = string.Empty;
    }
}
