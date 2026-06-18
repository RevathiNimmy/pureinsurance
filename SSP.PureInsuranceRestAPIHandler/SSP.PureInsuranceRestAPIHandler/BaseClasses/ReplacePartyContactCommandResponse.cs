using System.Collections.Generic;
using System.Linq;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ReplacePartyContactCommandResponse
    {
        public IEnumerable<ReplacePartyContactCommandBaseResponse> ReplacePartyContactResponse { get; set; } = Enumerable.Empty<ReplacePartyContactCommandBaseResponse>();
    }
}
