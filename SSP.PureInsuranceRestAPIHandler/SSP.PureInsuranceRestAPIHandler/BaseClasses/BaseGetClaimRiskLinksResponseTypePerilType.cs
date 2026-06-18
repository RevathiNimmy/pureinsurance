using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetClaimRiskLinksResponseTypePerilType
    {
        //[DBCol("peril_type_code")]
        public string Code { get; set; }
        //[DBCol("peril_type_description")]
        public string Description { get; set; }
        public System.Collections.Generic.List<BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType> RecoveryType { get; set; }
        public System.Collections.Generic.List<BaseGetClaimRiskLinksResponseTypePerilTypeReserveType> ReserveType { get; set; }
        //[DBCol("sum_insured")]
        public decimal SumInsured { get; set; }
    }
}
