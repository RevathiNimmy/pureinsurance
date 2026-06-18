using System.Collections.Generic;
using System.Linq;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalSelectionCommandResponse
    {
        public IEnumerable<RunRenewalSelectionCommandBaseResponse> RunRenewalSelectionResponse { get; set; } = Enumerable.Empty<RunRenewalSelectionCommandBaseResponse>();
    }
}
