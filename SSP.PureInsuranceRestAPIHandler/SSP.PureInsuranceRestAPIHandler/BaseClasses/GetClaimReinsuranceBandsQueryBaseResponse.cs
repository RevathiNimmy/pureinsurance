using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetClaimReinsuranceBandsQueryBaseResponse : BasePagedResponse
    {
        public List<BaseGetClaimReinsuranceBandsResponseTypeRow> ReinsuranceBands { get; set; }
    }
}
