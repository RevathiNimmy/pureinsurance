using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetCoinsuranceDefaultsQueryResponse : BasePagedResponse
    {
        public List<BaseGetCoinsuranceDefaultsResponseTypeRow> Defaults { get; set; }
    }
}
