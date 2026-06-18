using System.Collections.Generic;
using System.Linq;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalAcceptCommandResponse
    {
        public IEnumerable<RunRenewalAcceptCommandBaseResponse> RunRenewalAcceptResponse { get; set; } = Enumerable.Empty<RunRenewalAcceptCommandBaseResponse>();
    }
}
