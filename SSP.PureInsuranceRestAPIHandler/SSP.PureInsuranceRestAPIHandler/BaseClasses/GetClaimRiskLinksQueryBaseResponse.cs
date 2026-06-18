using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimRiskLinksQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimRiskLinksResponseTypePerilType> PerilType { get; set; }
    }
}
