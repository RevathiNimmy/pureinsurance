using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses //.Application.Account.Queries.FindAccountsQuery
{
    public class FindAccountsQueryBaseResponse : BaseResponseType
    {
        public System.Collections.Generic.List<BaseFindAccountsResponseTypeRow> Accounts { get; set; }
    }
}
