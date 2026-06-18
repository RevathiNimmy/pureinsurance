namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetRIVersionResponseTypeRIVersionsRow
    {
        //[DBCol("Effective_Date")]
        public System.DateTime RIEffectiveDate { get; set; }
        //[DBCol("Description")]
        public string VersionDescription { get; set; }
        //[DBCol("version_id")]
        public int VersionID { get; set; }
    }
}
