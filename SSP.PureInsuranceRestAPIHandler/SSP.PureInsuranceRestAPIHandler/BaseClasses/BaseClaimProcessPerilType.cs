using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseClaimProcessPerilType
    {
        public string TypeCode { get; set; }
        public string Description { get; set; }
        public System.Collections.Generic.List<BaseClaimProcessPerilReserveType> Reserve { get; set; }
        public System.Collections.Generic.List<BaseClaimProcessPerilRecoveryType> Recovery { get; set; }
    }
}
