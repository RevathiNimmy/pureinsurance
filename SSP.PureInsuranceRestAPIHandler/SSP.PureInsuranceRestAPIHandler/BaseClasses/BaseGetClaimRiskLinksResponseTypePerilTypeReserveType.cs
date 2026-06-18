namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimRiskLinksResponseTypePerilTypeReserveType
    {
        //[DBCol("peril_type_code")]
        public string Code { get; set; }
        //[DBCol("Description")]
        public string Description { get; set; }
        //[DBCol("reserve_type_code")] 
        public string ReserveCode { get; set; }
    }
}
