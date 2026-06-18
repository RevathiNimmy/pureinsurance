using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindUsersQueryBaseResponse : BasePagedResponse
    {
        public List<BaseFindUsersResponseTypeRow> Users { get; set; }
    }
}
