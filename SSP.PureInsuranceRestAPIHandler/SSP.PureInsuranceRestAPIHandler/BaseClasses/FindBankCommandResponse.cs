using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Commands.FindBank
{
    public class FindBankCommandResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseFindBankResponseTypeRow> Bank { get; set; }
    }
}
