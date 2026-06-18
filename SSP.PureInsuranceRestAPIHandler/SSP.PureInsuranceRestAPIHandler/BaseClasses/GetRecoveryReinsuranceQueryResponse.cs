using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRecoveryReinsuranceQueryResponse : BasePagedResponse
    {
        public List<BaseGetRecoveryReinsuranceResponseTypeRow> Reinsurances { get; set; }
    }
}
