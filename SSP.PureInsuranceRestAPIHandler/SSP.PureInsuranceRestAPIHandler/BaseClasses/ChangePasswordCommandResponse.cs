using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class ChangePasswordCommandResponse
    {
        public List<ChangePasswordCommandBaseResponse> ChangePasswordResponse { get; set; } = new List<ChangePasswordCommandBaseResponse>();
    }
}
