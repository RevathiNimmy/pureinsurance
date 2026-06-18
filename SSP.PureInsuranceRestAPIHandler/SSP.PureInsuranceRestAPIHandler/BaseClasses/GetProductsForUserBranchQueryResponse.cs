using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetProductsForUserBranchQueryResponse : BasePagedResponse
    {
        public List<BaseGetProductsForUserBranchResponseTypeProductsRow> Products { get; set; }
    }
}
