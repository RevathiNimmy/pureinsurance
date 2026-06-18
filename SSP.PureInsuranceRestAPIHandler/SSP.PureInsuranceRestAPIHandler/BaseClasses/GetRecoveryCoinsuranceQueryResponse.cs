using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetRecoveryCoinsuranceQueryResponse : BasePagedResponse
    {
        public List<BaseGetRecoveryCoinsuranceResponseTypeRow> Coinsurances { get; set; }
    }
}
