namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType
    {
        //[DBCol("recovery_type_code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("is_salvage")]
        public int IsSalvage { get; set; }
    }
}
