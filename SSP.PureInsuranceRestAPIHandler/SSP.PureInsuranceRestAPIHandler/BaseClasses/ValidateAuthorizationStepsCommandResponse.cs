using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.ValidateAuthorizationSteps
{
    public class ValidateAuthorizationStepsCommandResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseValidateAuthorizationStepsResponseType> ValidateAuthorizationSteps { get; set; }
    }
}
