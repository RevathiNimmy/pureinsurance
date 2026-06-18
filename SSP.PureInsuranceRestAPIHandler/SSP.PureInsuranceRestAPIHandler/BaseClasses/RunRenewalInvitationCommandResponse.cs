using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class RunRenewalInvitationCommandResponse
    {
        public List<RunRenewalInvitationCommandBaseResponse> RunRenewalInvitationResponse { get; set; } = new List<RunRenewalInvitationCommandBaseResponse>();
    }
}
