using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetInstalmentQuotesQueryResponse : BasePagedResponse
    {
        public GetInstalmentQuotesQueryBaseResponse GetInstalmentQuotesResponse { get; set; }
    }
}
