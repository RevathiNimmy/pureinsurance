using System.Collections.Generic;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GetReferredPaymentsQueryResponse : BasePagedResponse
    {
        public List<BaseGetReferredPaymentsResponseTypeRow> CashListItems { get; set; }
    }
}
